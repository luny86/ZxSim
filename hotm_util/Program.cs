using Builder;
using ZX.Util;

namespace HOTM;


class HotmUtil : IBuildable
{
    private MemoryMap _map = null!;
    private GraphicDecoder _graphicDecoder = null!;
    private ObjectDecoder _objectDecoder = null!;

    private RoomDecoder _roomDecoder = null!;
    private ResponseDecoder _responseDecoder = null!;

    public HotmUtil()
    {
        try
        {

            Creator creator = new Creator(true);
            creator.BuildAll(this);
            Console.Write(creator);
        }
        catch (InvalidOperationException e)
        {
            Console.Write($"Error {e.Message}");
        }
    }

    IList<IBuildable> IBuildable.CreateBuildables()
    {
        _graphicDecoder = new GraphicDecoder();
        _objectDecoder = new ObjectDecoder();

        return new List<IBuildable>()
        {
            _graphicDecoder,
            _objectDecoder
        };
    }

    void IBuildable.AskForDependents(IRequests requests)
    {

    }

    void IBuildable.RegisterObjects(IDependencyPool dependencies)
    {
        byte[] memory = ReadFile();
        _map = new MemoryMap(0x4000, memory);
        _map.AddRange(MemoryChunkNames.Dictionary, 0x5e29, 0x984);
        _map.AddRange(MemoryChunkNames.WordTokenTableA, 0x6ab6, 0x407);
        _map.AddRange(MemoryChunkNames.WordTokenTableDescriptions, 0x679f, 0x317);
        _map.AddRange(MemoryChunkNames.Bitmaps, 0xbfb8, 0x3b20);
        _map.AddRange(MemoryChunkNames.Sprites, 0xbbb2, 0x406);
        _map.AddRange(MemoryChunkNames.ObjectData, 0x6ebd, 0x5eb);
        _map.AddRange(MemoryChunkNames.Rooms, 0x7137, 0x371);
        _map.AddRange(MemoryChunkNames.ResponseParser, 0x661e, 0x188);

        dependencies.Add("HOTM.MemoryMap",
            typeof(ZX.Util.IMemoryMap),
            _map);

        WordDecoder decoder = new WordDecoder(
            _map[MemoryChunkNames.Dictionary],
            _map[MemoryChunkNames.WordTokenTableA]
        );

        dependencies.Add(MemoryChunkNames.WordDecoder,
            typeof(IWordDecoder),
            decoder);

        _responseDecoder = new ResponseDecoder(
            _map[MemoryChunkNames.ResponseParser],
            decoder
        );

        decoder = new WordDecoder(
            _map[MemoryChunkNames.Dictionary],
            _map[MemoryChunkNames.WordTokenTableDescriptions]);

        _roomDecoder = new RoomDecoder(_map[MemoryChunkNames.Rooms], decoder);
    }

    void IBuildable.DependentsMet(IDependencies dependencies)
    {
    }

    void IBuildable.EndBuild()
    {
    }

    private byte[] ReadFile()
    {
        byte[] memory = new byte[49152];

        using (BinaryReader reader = new BinaryReader(
            new FileStream("../resources/hotm.bin", FileMode.Open)))
        {
            reader.Read(memory, 0, 49152);
        }

        return memory;
    }

    public void Run()
    {
        Console.WriteLine("HOTM...");
        Console.WriteLine(_responseDecoder.ToString());
    }

    static void Main()
    {
        HotmUtil util = new HotmUtil();
        util.Run();
    }
}