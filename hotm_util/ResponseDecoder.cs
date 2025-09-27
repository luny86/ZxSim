using System.Text;
using System.Collections;
using System.Collections.Generic;
using ZX.Util;

namespace HOTM;

// Subsection rules:
// [00] = room number / 00 - ignore
// [01] = 00 - respond with [03]
//      else - 
//
//

internal class ResponseDecoder : IEnumerable<string>
{
    private enum ReadingMode
    {
        Reset,
        NewSection,
        Sections,
        SubSection
    }

    private class Enumerator : IEnumerator<string>
    {
        private const int ItemSize = 4;
        private const int _start = 0;
        private IChunk _interactions = null!;
        private IWordDecoder _responseDecoder = null!;

        private ReadingMode _mode;
        // Start  of  current section.
        private int _index = _start;
        // Current  position within  section  being  read.
        private int _subPos = 0;
        private int _count = 0;

        private bool _disposed = false;

        private int Count => (_interactions[_index] & 0xfc) >> 2;

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

                }

                _disposed = true;
            }
        }

        public Enumerator(IChunk interactions, IWordDecoder responses)
        {
            _interactions = interactions;
            _responseDecoder = responses;
            Reset();
        }

        private string DebugStr => $" : {Address} {_mode} {_index:x} {_interactions.Start + _subPos:x} {_subPos:x} {_count}";
        private void Debug(string prefix) { Console.Write(prefix); Console.WriteLine(DebugStr); }

        private string Address => $"${_interactions.Start + _subPos:x4}";

        // Noun expected  for  each  section.
        private string Noun => _responseDecoder.GetWord(_interactions[_index + 1], _interactions[_index + 2]);

        private string ResponseWord => _responseDecoder.GetWord(_interactions[_subPos + 1], _interactions[_subPos + 2]);

        private string ResponseString => _responseDecoder.DecodeString(_interactions[_subPos + 3]);

        string IEnumerator<string>.Current
        {
            get
            {
                StringBuilder result = new StringBuilder();

                switch (_mode)
                {
                    case ReadingMode.NewSection:
                        result.AppendLine($"N ${_interactions.Start + _index:x4} {Noun}");
                        result.Append($"B ${_interactions.Start + _index:x4},4 Size(${_interactions[_index]:x2})");
                        _mode = ReadingMode.SubSection;
                        break;

                    case ReadingMode.SubSection:
                        if (--_count == 0)
                        {
                            int i = _interactions[_subPos];
                            string r = i == 0 ? "" : _responseDecoder.DecodeString(i);
                            result.AppendLine($"B {Address},4 {r}");
                            _mode = ReadingMode.NewSection;
                        }
                        else
                        {
                            int room = _interactions[_subPos];
                            string roomStr = room == 0 ? "" : $"Room(${room:x2}) ";
                            result.Append($"B {Address},4 {roomStr}{ResponseWord} =>  {ResponseString}");
                        }
                        break;

                }

                return result.ToString();
            }
        }

        private object CurrentInternal
        {
            get { return ((IEnumerator<string>)this).Current; }
        }

        object IEnumerator.Current { get => CurrentInternal; }

        public bool MoveNext()
        {
            switch (_mode)
            {
                case ReadingMode.Reset:
                    _index = _start;
                    _count = Count;
                    _subPos = _index;
                    _mode = ReadingMode.NewSection;
                    break;

                case ReadingMode.NewSection:
                    _index += _interactions[_index];
                    _count = Count;
                    _subPos = _index;
                    break;


                case ReadingMode.SubSection:
                    _subPos += 4;
                    break;
            }

            return _interactions[_index] != 0;
        }

        public void Reset()
        {
            _mode = ReadingMode.Reset;
            _count = Count;
            _index = _start;
            _subPos = _index + 4;
        }
    }

    private IChunk _responses = null!;
    private IWordDecoder _words = null!;

    public ResponseDecoder(IChunk responses, IWordDecoder words)
    {
        _responses = responses;
        _words = words;
    }

    public IEnumerator<string> GetEnumerator()
    {
        return GetInternal();
    }

    private IEnumerator<string> GetInternal()
    {
        return new Enumerator(_responses, _words);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetInternal();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        foreach (string s in this)
        {
            sb.AppendLine(s);
        }

        return sb.ToString();
    }
}