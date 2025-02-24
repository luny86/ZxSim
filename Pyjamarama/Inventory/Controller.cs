
using Builder;
using ZX;
using ZX.Drawing;
using ZX.Game;
using ZX.Platform;
using ZX.Util.Observing;

namespace Pyjamarama.Inventory
{
    /// <summary>
    /// Inventory controller 
    /// </summary>
    internal class Controller : IGameItem, IBuildable, 
        ZX.Util.Observing.IObservable<InventoryStats>
    {
        #region Private Members

        private IFactory _factory = null!;
        private ZX.Platform.IFactory _platformFactory = null!;
        private ZX.Drawing.IFactory _drawingFactory = null!;
        private IScreen _screen = null!;

        private Layer _layer = null!;
        private ISurface _surface = null!;

        private Subscriber<InventoryStats> _subscriber;

        private InventoryStats _stats = new InventoryStats();
        #endregion

        #region Construction

        public Controller()
        {
            _subscriber = new Subscriber<InventoryStats>(this);
        }

        #endregion

        #region IBuildable


        IList<IBuildable>? IBuildable.CreateBuildables()
        {
            return null;
        }

        void IBuildable.AskForDependents(IRequests requests)
        {
            requests.AddRequest("Pyjamarama.Factory", typeof(IFactory));
            requests.AddRequest(ZX.Drawing.ClassNames.Screen, typeof(ZX.Drawing.IScreen));
            requests.AddRequest(ZX.Drawing.ClassNames.Factory, typeof(ZX.Drawing.IFactory));
            requests.AddRequest(ZX.Platform.ClassNames.Factory, typeof(ZX.Platform.IFactory));
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {
            _screen = dependencies.TryGetInstance<IScreen>(ZX.Drawing.ClassNames.Screen);
            _platformFactory = dependencies.TryGetInstance<ZX.Platform.IFactory>(ZX.Platform.ClassNames.Factory);
            _drawingFactory = dependencies.TryGetInstance<ZX.Drawing.IFactory>(ZX.Drawing.ClassNames.Factory);
            _factory = dependencies.TryGetInstance<IFactory>("Pyjamarama.Factory");

            _surface = _platformFactory.CreateSurface();
        }

        void IBuildable.EndBuild()
        {
            IDrawer pocketDrawer = CreatePockets();
            IDrawer livesDrawer = _drawingFactory.CreateBitmapDrawer(MemoryChunkNames.LivesBitmaps, 0x10, 0x10);
            ISizeableDrawer energy = _drawingFactory.CreateBitmapDrawer(MemoryChunkNames.MilkGlass, 0x18, 0x20)
                as ISizeableDrawer 
                ?? throw new InvalidOperationException("Unable to create milk bitmap.");

            if(energy is IAttribute attribute)
            {
                Palette.SetAttribute(Palette.BrightWhite, Palette.Black, attribute);
            }
            _layer = new Layer(_surface, pocketDrawer, livesDrawer, energy);
            _screen.AddLayer(_layer);    
            CreateAndDrawScoreBoard();

            if(this is ZX.Util.Observing.IObservable<InventoryStats> obserable)
            {
                obserable.Subscribe(_layer);
            }
        }

        private void CreateAndDrawScoreBoard()
        {
            IDrawer drawer = _factory.CreateFurnitureDrawer(MemoryChunkNames.TileBitmaps, MemoryChunkNames.InventoryFurniture);

            drawer.Draw(_surface, 0, 1, 0);
        }

        private IDrawer CreatePockets()
        {
            return _drawingFactory.CreateBitmapDrawer(MemoryChunkNames.ObjectBitmaps, 0x10, 0x10);
        }
        #endregion

        #region IGameItem

        void IGameStatic.NewGame()
        {
           _stats = new InventoryStats
            {
                pocket1 = 8,
                pocket2 = 14,
                livesLeft = 3
            };
            _subscriber.OnChanged(_stats);
        }

        void IGameStatic.NewLevel()
        {
        }

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {

        }

        void IGameItem.Update()
        {
 
        }

        #endregion

        #region IObservable

        IUnsubscriber<InventoryStats> ZX.Util.Observing.IObservable<InventoryStats>.Subscribe(ZX.Util.Observing.IObserver<InventoryStats> observer)
        {
            _subscriber.Subscribe(observer);
            return _subscriber;
        }

        #endregion


    }
}