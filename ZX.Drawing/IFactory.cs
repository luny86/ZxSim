
using KUtil;

namespace ZX.Drawing
{
    public interface IFactory
    {
        IDrawer CreateTileDrawer(IChunk tileMemory);
    }
}