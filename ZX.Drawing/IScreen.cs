
using ZX.Platform;

namespace ZX.Drawing
{
    /// <summary>
    /// Describes a screen.
    /// </summary>
    public interface IScreen
    {
        ISurface Main { set; }

        void AddLayer(ILayer layer);

        void Update();
    }
}