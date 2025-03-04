
using ZX.Drawing;
using ZX.Platform;

namespace Pyjamarama.House
{
    internal class ObjectLayer : Layer
    {
        const int SlotYOffset = 0x10;

        private readonly IDrawer _drawer;

        /// <summary>
        /// Reference to the current object.
        /// </summary>
        public IObjectSlot Slot
        {
            get;
            set;
        } = null!;

        public ObjectLayer(IDrawer drawer, ISurface surface)
        : base("Object", surface, (int)LayerZOrders.Object)
        {
            Surface.Create(0x10, 0x10);
            _drawer = drawer;
        }

        public override void Update()
        {
            X = Slot.X;
            Y = Slot.Y+SlotYOffset;

            _drawer.Draw(Surface, Slot.ObjectIndex, 0,0);
        }
    }
}