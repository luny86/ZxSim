
using System.Text;
using System.Collections;
using System.Collections.Generic;
using ZX.Util;

namespace HOTM;

/// <summary>
/// Converts the rooms into displayable data.
/// </summary>
class RoomDecoder : IEnumerable<string>
{
    static string[] Exits = ["NW", "N", "NW", "SW", "S", "SE"];
    static string[] ExitTypes =
    [
        "Archway",
        "Cave entrance",
        "Door shut",
        "Door open"
    ];

    static string[] ExitDirections =
    [
        "", // Same level
        "up",
        "down"
    ];

    private class Enumerator : IEnumerator<string>
    {
        private const int _start = 0;
        private IChunk _rooms = null!;
        private IWordDecoder _words = null!;
        private int _index = _start;
        private bool _reset = false;

        private bool _disposed = false;

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

        public Enumerator(IChunk rooms, IWordDecoder words)
        {
            _rooms = rooms;
            _words = words;
            _reset = true;
        }

        private string Name()
        {
            int s = _rooms[_index];
            s <<= 1;
            s += (((_rooms[_index + 1] & 0x80) == 0x80) ? 1 : 0);
            s &= 0x3f;

            return _words.DecodeString(s);
        }

        private int Exits(int index)
        {
            index += 2;
            int e = 0;
            byte mask = _rooms[index];
            index++;
            for (int i = 0; i < 6; i++)
            {
                if ((mask & 128) == 128)
                {
                    e++;
                }

                mask <<= 1;
            }
            e = (e & 1) + (e >> 1);
            index += e;

            return index;
        }

        string IEnumerator<string>.Current
        {
            get
            {
                int s = _index;

                return $"B ${_rooms.Start + s:x4} {Name()}";
            }
        }

        private object CurrentInternal
        {
            get { return ((IEnumerator<string>)this).Current; }
        }

        object IEnumerator.Current { get => CurrentInternal; }

        public bool MoveNext()
        {
            if (_reset)
            {
                _reset = false;
            }
            else
            {
                _index = Exits(_index);
            }

            return _index < _rooms.Length;
        }

        public void Reset()
        {
            _index = _start;
        }
    }

    private IChunk _rooms = null!;
    private IWordDecoder _words = null!;

    public RoomDecoder(IChunk rooms, IWordDecoder words)
    {
        _rooms = rooms;
        _words = words;
    }

    public IEnumerator<string> GetEnumerator()
    {
        return GetInternal();
    }

    private IEnumerator<string> GetInternal()
    {
        return new Enumerator(_rooms, _words);
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