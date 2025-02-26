
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

        private int HeadFrame
        {
            get
            {
                int frame = Frame;

                if(HeadTurned)
                {
                    if(frame < 0x10)
                    {
                        frame += 0x3e;
                    }
                    else
                    {
                        frame += 0x2f;
                    }
                }

                return frame;
            }
        }

        public bool HeadTurned
        {
            get;
            set;
        }

        public override void Update()
        {
            _drawer.Draw(Surface, HeadFrame, 0, 0);
            _drawer.Draw(Surface, Frame+1, 0, 0x10);
        }

    }
}