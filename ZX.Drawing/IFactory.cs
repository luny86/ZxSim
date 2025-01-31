
using ZX.Util;

namespace ZX.Drawing
{
    public interface IFactory
    {
        IDrawer CreateTileDrawer(IChunk tileMemory);
    }
}