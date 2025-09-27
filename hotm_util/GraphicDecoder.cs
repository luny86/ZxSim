using Builder;
using ZX.Util;
using IronSoftware.Drawing;

namespace HOTM;

internal class GraphicDecoder : IBuildable
{
    #region Private Types

    private const int BITMAP = 0;
    private const int WIDTH = 3;
    private const int HEIGHT = 2;

    record Sprite(int Width, int Height, int Offset);
    #endregion

    #region Dependencies

    private IChunk _bitmaps = null!;
    private IChunk _sprites = null!;

    #endregion

    #region Methods

    public GraphicDecoder()
    {
    }

    public void Create(string filename, int spriteIndex)
    {
        Sprite sprite = GetBitmap(spriteIndex);
        if (sprite.Width > 0 && sprite.Height > 0)
        {
            AnyBitmap bitmap = DrawBitmap(sprite);

            bitmap.ExportFile(filename, AnyBitmap.ImageFormat.Png);
        }
    }

    private Sprite GetBitmap(int index)
    {
        int offset = (index * 4) + 4;

        return new Sprite(
            (_sprites[offset + WIDTH] & 0x7f) * 8,
            (_sprites[offset + HEIGHT] & 0x7f) * 8,
            (int)(_sprites.Word(offset + BITMAP) - _bitmaps.Start));
    }

    private AnyBitmap DrawBitmap(Sprite sprite)
    {
        AnyBitmap bitmap = new AnyBitmap(sprite.Width, sprite.Height);
        int addr = sprite.Offset;

        for (int y = 0; y < sprite.Height; y++)
        {
            for (int x = 0; x < sprite.Width; x += 8)
            {
                byte b = _bitmaps[addr++];
                bitmap.SetPixel(x, y, Fore(((b & (byte)0x80) != 0)));
                bitmap.SetPixel(x + 1, y, Fore((b & (byte)0x20) != 0));
                bitmap.SetPixel(x + 2, y, Fore((b & (byte)0x08) != 0));
                bitmap.SetPixel(x + 3, y, Fore((b & (byte)0x02) != 0));
                bitmap.SetPixel(x + 4, y, Fore((b & (byte)0x40) != 0));
                bitmap.SetPixel(x + 5, y, Fore((b & (byte)0x10) != 0));
                bitmap.SetPixel(x + 6, y, Fore((b & (byte)0x04) != 0));
                bitmap.SetPixel(x + 7, y, Fore((b & (byte)0x01) != 0));
            }
        }

        return bitmap;
    }

    private static Color Fore(bool fore) { return fore ? Color.White : Color.Black; }
    #endregion

    #region IBuildable

    IList<IBuildable> IBuildable.CreateBuildables()
    {
        return new List<IBuildable>()
        {
        };
    }

    void IBuildable.AskForDependents(IRequests requests)
    {
        requests.AddRequest(MemoryChunkNames.MemoryMap, typeof(IMemoryMap));
    }

    void IBuildable.RegisterObjects(IDependencyPool dependencies)
    {
    }

    void IBuildable.DependentsMet(IDependencies dependencies)
    {
        IMemoryMap map = dependencies.TryGetInstance<IMemoryMap>(MemoryChunkNames.MemoryMap);
        _bitmaps = map[MemoryChunkNames.Bitmaps];
        _sprites = map[MemoryChunkNames.Sprites];

    }

    void IBuildable.EndBuild()
    {

    }

    #endregion    
}