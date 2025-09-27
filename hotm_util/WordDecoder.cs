using System;
using System.Text;
using Builder;
using ZX.Util;

namespace HOTM;

/// <summary>
/// Decodes HOTM words strings into local strings.
/// </summary>
/// <remarks>
/// The words are stored in a word table of 2 byte data each pair
/// representing a word. Each 'string' is represented by an index
/// based on the order of the string in the table.
/// </remarks>
internal class WordDecoder : IWordDecoder
{
    private enum TokenCodes : byte
    {
        EndOfString = 0xff,
        EndOfStringAlt = 0,
        Word = 0xf7,
        Parameter = 0xfe,
        AOrAn = 0xfc,
        NewLine = 0xfd,
        Char = 0xfb,
        Tab = 0xf9,
        SpaceOrEnd = 0xfa,
        StringReference = 0xf8
    };

    private IChunk _dictionary = null!;
    private IChunk _wordTokenTable = null!;

    public WordDecoder(IChunk dictionary, IChunk wordTable)
    {
        _dictionary = dictionary;
        _wordTokenTable = wordTable;
    }

    public string DecodeString(int index)
    {
        int i = GetStringData(index);

        return Decode(i);
    }

    //
    // Scan string at offset 'i'
    private string Decode(int index)
    {
        StringBuilder sb = new StringBuilder();
        bool done = false;

        while (!done)
        {
            switch ((TokenCodes)_wordTokenTable[index])
            {
                case TokenCodes.EndOfString:
                case TokenCodes.EndOfStringAlt:
                    done = true;
                    break;

                case TokenCodes.Word:
                    // Print word $AF58
                    sb.Append("{AF58}");
                    break;

                case TokenCodes.Parameter:
                    sb.Append("{$1}");
                    break;

                case TokenCodes.AOrAn:
                    sb.Append("{an}");
                    break;

                case TokenCodes.NewLine:
                    sb.Append("{newline}");
                    break;

                case TokenCodes.Char:
                    index++;
                    sb.Append($"{(char)(index)}");
                    break;

                case TokenCodes.Tab:
                    sb.Append("{tab}");
                    break;

                case TokenCodes.SpaceOrEnd:
                    sb.Append("{space or end}");
                    break;

                case TokenCodes.StringReference:
                    sb.Append($"{{$16,{_wordTokenTable[index + 1]}, {_wordTokenTable[index + 2]}}}");
                    index += 2;
                    break;

                default:
                    sb.Append(GetWord(_wordTokenTable[index], _wordTokenTable[index + 1]));
                    index++;
                    break;
            }

            if (!done)
            {
                index++;
                if (_wordTokenTable[index] != 0 && _wordTokenTable[index] != 0xff)
                {
                    sb.Append(" ");
                }
            }
        }

        return sb.ToString();
    }

    public string GetWord(int size, int index)
    {
        ASCIIEncoding ascii = new ASCIIEncoding();
        ushort addr = _dictionary.Word(size * 2);
        int offset = (addr - _dictionary.Start) + ((index - 1) * size);

        StringBuilder sb = new StringBuilder(size);
        for (int i = 0; i < size; i++)
        {
            char[] a = ascii.GetChars(new byte[] { (byte)(_dictionary[offset + i] & (byte)0x7f) });
            char v = a[0] > (char)31 ? a[0] : '@';

            sb.Append(v);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Table always leads with a table of pointers.
    /// In HOTM, these always holds info on the single table,
    /// but can be extended to hold more. This method
    /// works out which table, then calculates the address
    /// of the string required.
    /// </summary>
    /// <param name="index">Index of string to decode.</param>
    /// <returns>Offset 'address' of string.</returns>
    private int GetStringData(int index)
    {
        const int size = 3;
        ushort offset = 0;
        int i = 0;

        while (true)
        {
            if (_wordTokenTable[i + size] > index)
            {
                offset = _wordTokenTable.Word(i + 1);
                break;
            }

            i += size;
        }

        int start = 0;
        i = ((int)offset) - _wordTokenTable.Start;
        while (index > 0)
        {
            // End of string.
            if (_wordTokenTable[i] == 0)
            {
                index--;
                start = i + 1;
            }

            i++;
        }

        return start;
    }
}