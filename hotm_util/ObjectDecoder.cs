using Builder;
using ZX.Util;

namespace HOTM;

/// <summary>
/// Decodes object info stored in memory.
/// </summary>
internal class ObjectDecoder : IBuildable
{
    const int DataSize = 9; // Bytes

    // Structure offsets in bytes.
    const int WordSize = 2;
    const int WordIndex = 3;

    IChunk _objectData = null!;
    IWordDecoder _wordDecoder = null!;

    public string Name(int index)
    {
        int offset = index * DataSize;

        string result = _wordDecoder.GetWord(_objectData[offset + WordSize], _objectData[offset + WordIndex]);

        return result;
    }

    #region IBuildable
    IList<IBuildable> IBuildable.CreateBuildables()
    {
        return new List<IBuildable>();
    }

    void IBuildable.RegisterObjects(IDependencyPool dependencies)
    {

    }

    void IBuildable.AskForDependents(IRequests requests)
    {
        requests.AddRequest(MemoryChunkNames.WordDecoder, typeof(IWordDecoder));
        requests.AddRequest(MemoryChunkNames.MemoryMap, typeof(IMemoryMap));
    }

    void IBuildable.DependentsMet(IDependencies dependencies)
    {
        IMemoryMap map = dependencies.TryGetInstance<IMemoryMap>(MemoryChunkNames.MemoryMap);
        _objectData = map[MemoryChunkNames.ObjectData];

        _wordDecoder = dependencies.TryGetInstance<IWordDecoder>(MemoryChunkNames.WordDecoder);
    }

    void IBuildable.EndBuild()
    {
    }
    #endregion
}