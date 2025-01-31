
using KUtil;

namespace ZX.Drawing
{
    /// <summary>
    /// Main factory class for ZX.Drawing
    /// </summary>
    internal class Factory : IFactory
    {
        IDrawer IFactory.CreateTileDrawer(IChunk tileMemory)
        {
            return new TileDrawer(tileMemory);
        }
    }
}