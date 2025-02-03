
using GameEditorLib.Builder;
using ZX.Platform;
using ZX.Drawing;
using ZX.Util;

namespace Pyjamarama
{
    public class BackgroundLayer : Layer
    {
        private readonly IDrawer _tileDrawer = null!;

        public BackgroundLayer(IDrawer drawer, ISurface surface, int z)
        : base(surface, z)
        {
            _tileDrawer = drawer;

            Surface.Create(256,192);
        }
        
        public override void Update()
        {
            Surface.Fill(new Rgba(0.0f, 0.0f, 0.0f, 0.0f));
            _tileDrawer.Draw(Surface, 4, 10,10);
        }
    }
}