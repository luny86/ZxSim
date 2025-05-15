
using System.Collections.Generic;

namespace ZX.Util
{
    /// <summary>
    /// A collection of memory chunks allowing users
    /// to get a byte from any chunk simply by using
    /// an asbolute memory address.
    /// </summary>
    internal class ChunkCollection
    {
        private List<IChunk> _memoryMap = new List<IChunk>();

        public ChunkCollection(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
        }

        public byte this[int address]
        {
            get
            {
                bool found = false;
                byte data = 0xff;

                foreach (IChunk chunk in _memoryMap)
                {
                    if (chunk.IsInRange(address))
                    {
                        data = chunk[address - chunk.Start];
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    throw new IndexOutOfRangeException($"Address {address} not in range for memory chunk {Name}.");
                }

                return data;
            }
        }

        public void Add(IChunk chunk)
        {
            _memoryMap.Add(chunk);
        }

    }
}