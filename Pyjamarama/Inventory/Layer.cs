
using ZX.Drawing;
using ZX.Platform;
using ZX;
using ZX.Util;
using Observing = ZX.Util.Observing;

namespace Pyjamarama.Inventory
{
    /// <summary>
    /// Layer for drawing the inventory and all of its contents.
    /// </summary>
    internal class Layer : ZX.Drawing.Layer, Observing.IObserver<InventoryStats>
    {
        #region Private Members
        
        private const int _width = 256;
        private const int _height = 6*8;

        private InventoryStats _stats;

        private readonly IDrawer _pocketDrawer;
        private readonly IDrawer _livesDrawer;
        private readonly IDrawer _textDrawer;
        private readonly ISizeableDrawer _energy;

        #endregion

        #region Construction

        public Layer(
            ISurface surface, 
            IDrawer pocketDrawer, 
            IDrawer livesDrawer,
            IDrawer textDrawer,
            ISizeableDrawer energy)
        :base("Inventory", surface, (int)LayerZOrders.Inventory)
        {
            _stats = new InventoryStats();

            _pocketDrawer = pocketDrawer;
            _livesDrawer = livesDrawer;
            _textDrawer = textDrawer;
            _energy = energy;

            Surface.Create(_width, _height);
        }

        #endregion

        #region Layer 

        public override void Update()
        {
            RedrawPocket(Palette.BrightCyan, _stats.pocket1, 0x90, 0);
            RedrawPocket(Palette.BrightYellow, _stats.pocket2, 0x90, 16);
            DrawText();
            RedrawLives();
            RedrawEnergy();
        }

        private void RedrawPocket(Rgba ink, int index, int x, int y)
        {
            IAttribute? attribute = _pocketDrawer as ZX.IAttribute;
            
            if(attribute != null)
            {
                Palette.SetAttribute(
                    ink, 
                    Palette.Black, 
                    attribute);
            }

            _pocketDrawer.Draw(Surface, index, x, y);
        }

        private void DrawText()
        {
            const int x = 21;

            IAttribute attribute = _textDrawer as IAttribute ?? 
                throw new InvalidOperationException("_textDrawer should implement IAttribute");

            attribute.Ink = Palette.BrightGreen;
            _textDrawer.Draw(Surface, _stats.pocket1, x,0);
            attribute.Ink = Palette.BrightCyan;
            _textDrawer.Draw(Surface, _stats.pocket2, x,2);
        }

        private void RedrawLives()
        {
            const int x = 0;
            const int y = 0x10;
            const int w = 0x10;
            const int h = 0x10;

            Surface.FillRect(new Rectangle(X,y,w*3, h), Palette.Black);
            for(int i = 0; i < _stats.livesLeft; i++)
            {
                _livesDrawer.Draw(Surface, 0, x+(i*w), y);
            }
        }

        private void RedrawEnergy()
        {
            _energy.PreClear = false;
            _energy.BlitRect = Rectangle.Empty;
            _energy.Draw(Surface, 1, 0x68, 0);

            Rectangle o = _energy.BlitRect;
            _energy.BlitRect = new Rectangle(
                0,
                o.H - _stats.Energy,
                o.W,
                o.H);
            _energy.Draw(Surface, 0, 0x68, 0);
        }
        #endregion

        #region IObserver

        void Observing.IObserver<InventoryStats>.ObserableChanged(InventoryStats newStats)
        {
            _stats = newStats;
            Update();
        }
        #endregion
    }
}