
using ZX.Util;

namespace ZX.Drawing
{
    public interface IFactory
    {
        IDrawer CreateTileDrawer(string tileChunkName);
        IDrawer CreateBitmapDrawer(string bitmapChunkName, int width, int height);
    }
}