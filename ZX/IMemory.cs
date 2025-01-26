
namespace ZX
{
    /// <summary>
    /// Describes the memory of a snapshot.
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
        byte[] this[int index] { get; }
    }
}