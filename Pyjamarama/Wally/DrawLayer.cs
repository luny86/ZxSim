
using ZX.Drawing;
using ZX.Platform;

namespace Pyjamarama.Wally
{
    /// <summary>
    /// Brings together an <see cref="ISurface"/> and an <see cref="IDrawer"/>
    /// for drawing bitmaps onto the screen.
    /// </summary>
    internal class DrawLayer : ZX.Drawing.Layer
    {
        private IDrawer _drawer = null!;

        public DrawLayer(IDrawer drawer, ISurface surface, int z)
        : base("Wally", surface, z)
        {
            _drawer = drawer;
        }

        public int Frame
        {
            get;
            set;
        }

        public override void Update()
        {
            _drawer.Draw(Surface, Frame, 0, 0);
        }
    }
}