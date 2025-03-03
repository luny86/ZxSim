
using ZX.Platform;

namespace ZX.Drawing
{
    public interface IFactory
    {
        IDrawer CreateTileDrawer(string tileChunkName);
        IDrawer CreateBitmapDrawer(string bitmapChunkName, int width, int height);

        IAnimationLayer CreateAnimationLayer(string name, IDrawer spriteDrawer, ISurface surface, int z);
        IAnimation CreateStaticAnimation(string name, int start, int end, int freq);
    }
}