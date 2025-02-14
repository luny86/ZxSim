
using ZX.Drawing;
using ZX.Platform;

namespace Pyjamarama.Wally
{
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