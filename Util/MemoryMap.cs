
using System.Collections.Generic;

namespace ZX.Util
{
    public class MemoryMap : IMemoryMap
    {
        private int _start;
        private long _size;
        private byte[] _memory;

        private Dictionary<string, KUtil.Chunk> _chunks = null!;

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="start">Real address of binary, when loaded on original platform.</param>
        /// <param name="binary">Image of binary.</param>
        public MemoryMap(int start, byte[] binary)
        {
            _start =start;
            _size = binary.Length;
            _memory = new byte[_size];
            Array.Copy(binary, _memory, _size);
        }

        public KUtil.IChunk this[string name]
        {
            get
            {
                return _chunks[name];
            }
        }

        public void AddRange(string name, int address, int length)
        {
            if(address < _start || address > _start+_size)
            {
                throw new ArgumentOutOfRangeException(nameof(address));
            }

            if(address + length > _start + _size)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            _chunks.Add(name, 
            new KUtil.Chunk(name, address, length, _memory));
        }

    }
}