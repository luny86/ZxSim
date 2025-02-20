
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

        #endregion

        #region Construction

        public Layer(ISurface surface, IDrawer pocketDrawer)
        :base("Inventory", surface, (int)LayerZOrders.Inventory)
        {
            _stats = new InventoryStats();

            _pocketDrawer = pocketDrawer;

            Surface.Create(_width, _height);
        }

        #endregion

        #region Layer 

        public override void Update()
        {
            UpdatePocket(Palette.BrightCyan, _stats.pocket1, 0x90, 0);
            UpdatePocket(Palette.BrightYellow, _stats.pocket2, 0x90, 16);
        }

        private void UpdatePocket(Rgba ink, int index, int x, int y)
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