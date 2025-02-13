
using ZX.Platform;

namespace ZX.Drawing
{
    /// <summary>
    /// Describes a screen.
    /// </summary>
    public interface IScreen
    {
        ILayer this[string name] { get; }

        ISurface Main { set; }

        void AddLayer(ILayer layer);

        void Update();
    }
}