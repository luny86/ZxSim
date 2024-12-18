

namespace KUtil
{
    /// <summary>
    /// Describes a chunk of memory
    /// </summary>
    public interface IChunk : IReadOnlyChunk
    {
        public new byte this[int index] { get; set; }
    }
}