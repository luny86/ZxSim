
using GameEditorLib.Builder;
using ZX.Platform;
using ZX.Drawing;
using ZX.Util;

namespace Pyjamarama
{
    public class BackgroundLayer : Layer
    {
        /// <summary>
        /// Background drawer for the game.
        /// </summary>
        private readonly IDrawer _drawer = null!;

        public BackgroundLayer(IDrawer drawer, string name, ISurface surface, int z)
        : base(name, surface, z)
        {
            _drawer = drawer;

            Surface.Create(256,192);
        }
        

        public int RoomIndex { get; set; }

        public override void Update()
        {
            Surface.Fill(new Rgba(0.0f, 0.0f, 0.0f, 0.0f));
            _drawer.Draw(Surface, RoomIndex, 10,10);
        }
    }
}