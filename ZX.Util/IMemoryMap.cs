
namespace ZX.Util
{
    public interface IMemoryMap
    {
        IChunk this[string name] { get; }
    }
}