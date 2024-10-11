using System;

namespace KUtil
{
    /// <summary>
    /// Holds information and data on a chunk of memory
    /// copied from the ram buffer.
    /// </summary>
    public class Chunk
    {
        const int RamStart = 0x4000;

        public Chunk(int start, int length, byte[] buffer)
        {
            Start = start;
            Length = length;

            byte[] memory = new byte[length];
            Array.Copy(buffer, start-RamStart, memory, 0, length);
            Memory = memory;
        }

        public byte this[int index]
        {
            get
            {
                return (index >=0 && index <Length) ? Memory[index] : (byte)0xff;
            }

            set
            {
                if(index >=0 && index < Length)
                {
                    Memory[index] = value;
                }
            }
        }

        public int Start { get; }
        public int Length { get; }

        private byte[] Memory { get; }
    }
}