
namespace ZX.Util
{
    public interface IMemoryMap
    {
        KUtil.IChunk this[string name] { get; }
    }
}