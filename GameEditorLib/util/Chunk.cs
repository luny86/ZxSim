using System;

namespace KUtil
{
    /// <summary>
    /// Holds information and data on a chunk of memory
    /// copied from the ram buffer.
    /// </summary>
    public class Chunk : IChunk
    {
        const int RamStart = 0x4000;

        public Chunk(string name, int start, int length, byte[] buffer)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            
            if(buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if(length < 1)
            {
                throw new ArgumentException($"Length cannot be less than 1");
            }

            Start = start;
            Length = length;

            byte[] memory = new byte[length];
            Array.Copy(buffer, start-RamStart, memory, 0, length);
            Memory = memory;
        }

        public string Name 
        {
             get;
             private set;
        }

        public static ushort Word(byte h, byte l)
        {
            return (ushort)(h + (l * 256));
        }

        public ushort Word(int index)
        {
            return (index >=0 && index <Length-1) ? 
                (ushort)(Memory[index] + (256 * Memory[index+1]))
                : (ushort)0xff;
        }

        public byte[] CopyRange(int index, int length)
        {
            byte[] copy = new byte[length];
            Array.Copy(Memory, index,  copy, 0, length);
            return copy;
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