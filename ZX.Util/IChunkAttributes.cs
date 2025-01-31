namespace ZX.Util
{
    /// <summary>
    /// Defines the aproperties used by a memory chunk.
    /// </summary>
    public interface IChunkAttributes
    {
        /// <summary>
        /// Name of chunk.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Start address of memory, using the real absolute 
        /// address in the original binary / hardware.
        /// </summary>
        int Start { get; }

        /// <summary>
        /// Number of bytes used by memory.
        /// </summary>
        int Length { get; }        
    }
}