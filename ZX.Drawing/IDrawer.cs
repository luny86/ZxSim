
using ZX.Platform;

namespace ZX.Drawing
{
    /// <summary>
    /// Base interface for a drawing object.
    /// </summary>
    public interface IDrawer
    {
        void Draw(ISurface surface, int index, int x, int y);
    }
}