

namespace ZX.Util
{
    /// <summary>
    /// Describes a Read only chunk of memory.
    /// </summary>
    public interface IReadOnlyChunk : IChunkAttributes
    {

        byte this[int index]
        {
            get;
        }

        /// <summary>
        /// Determines if  the given absolute address
        /// is in range of this chunk's memory.
        /// </summary>
        /// <param name="address">Address to check.</param>
        /// <returns>True if address is valid for this chunk.</returns>
        bool IsInRange(int address);

        /// <summary>
        /// Return the two bytes as a word (big endian)
        /// at index.
        /// </summary>
        /// <param name="index">Index of value.</param>
        /// <returns>The next two bytes at index, as a 16 bit number. Byte order
        /// is determined by implementation.</returns>
        ushort Word(int index);

        /// <summary>
        /// Get a copy of a bit of the chunk.
        /// </summary>
        /// <param name="index">Start index to copy from</param>
        /// <param name="length">Number of bytes to copy/</param>
        /// <returns>Copy of bytes</returns>
        byte[] CopyRange(int index, int length);

    }
}