
namespace ZX
{
    /// <summary>
    /// Describes the memory of a snapshot / binary.
    /// </summary>
    public interface IMemory
    {
        /// <summary>
        /// Name of snapshot loaded.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Start address (Spectrum memory map)
        /// of first byte.
        /// </summary>
        int Start { get; }

        /// <summary>
        /// Number of bytes in total.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Main indexer.
        /// </summary>
        /// <remarks>
        /// This starts from 0 within the chunk of memory defined.
        /// </remarks>
        byte[] this[int index] { get; }
    }
}