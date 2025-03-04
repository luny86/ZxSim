
using Bindings;
using Builder;
using Pyjamarama.House;
using System.Runtime.CompilerServices;
using ZX;
using ZX.Drawing;
using ZX.Game;
using ZX.Platform;
using Game = ZX.Game;


[assembly:InternalsVisibleTo("PyjamaramaTests")]

namespace Pyjamarama
{
    internal class Composition : IComposition, IBuildable, IGameItem
    {
        #region Flag names
        private class FlagsNames
        {
            public static string FuelCan = "FuelCan";
            public static string Bucket = "Bucket";
            public static string LiftCount = "F177";    // Value is for data test.
            public static string HelpSwitch = "F178";  
            public static string LaserGun = "F179"; 
            public static string LiftFloor = "F181";
            public static string MagLockDir = "MagLockDir";
            public static string ArcadeMode = "ArcadeMode";
        };

        #endregion

        #region Members

        private IFactory _factory = new Factory();

        private Wally.Controller _wallyController = null!;

        private Inventory.Controller _inventoryController = new Inventory.Controller();

        private IAnimationLayer _animationLayer = null!;

        private IAttributeTable _attributeTable = null!;

        ZX.Drawing.IFactory _drawFactory = null!;

        ZX.Platform.IFactory _platformFactory = null!;

        private ZX.Game.IFactory _gameFactory = null!;

        IScreen _screen = null!;

        Game.IGameProvider _gameProvider = null!;

        private RoomProvider _roomProvider = null!;

        private ActionController _actionController = null!;

	    private ZX.Game.IFlags _flags = null!;

        private IBindingManager _bindingManager = null!;

        private int _roomIndex = 0;
        #endregion

        string IComposition.Name => "Pyjamarama";

        #region IBuildable

        void IBuildable.AskForDependents(IRequests requests)
        {
            requests.AddRequest("ZX.Drawing.Screen", typeof(ZX.Drawing.IScreen));
            requests.AddRequest("ZX.Drawing.IFactory", typeof(ZX.Drawing.IFactory));
		    requests.AddRequest("ZX.Platform.IFactory", typeof(ZX.Platform.IFactory));
            requests.AddRequest(Game.ClassNames.GameProvider, typeof(Game.IGameProvider));
    		requests.AddRequest("ZX.Game.Flags",
				typeof(ZX.Game.IFlags));
		    requests.AddRequest("ZX.Game.Factory",
				typeof(ZX.Game.IFactory));
            requests.AddRequest(Bindings.ClassNames.BindingManager, typeof(IBindingManager));
        }

        IList<IBuildable>? IBuildable.CreateBuildables()
        {
            _attributeTable = _factory.GetAttributeTable();
            _wallyController = new Wally.Controller(_attributeTable);
            _roomProvider = new RoomProvider();

            ActionProvider actions = new ActionProvider();
            _actionController = new ActionController(actions);

            return new List<IBuildable>()
            {
                (_factory as IBuildable)  ?? throw new InvalidOperationException("Factory not buildable"),
                _wallyController,
                _inventoryController,
                _roomProvider,
                actions
            };
        }

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
            dependencies.Add(ClassNames.Factory, typeof(IFactory), _factory);
            dependencies.Add(ClassNames.Wally, typeof(IPlayer), _wallyController);
        }

        void IBuildable.DependentsMet(IDependencies dependencies)
        {
            _screen = dependencies.TryGetInstance("ZX.Drawing.Screen",
                typeof(ZX.Drawing.IScreen))
                as ZX.Drawing.IScreen
                ?? throw new InvalidOperationException("Unable to get dependency ZX.Drawing.Screen.");

            _drawFactory = dependencies.TryGetInstance("ZX.Drawing.IFactory",
                typeof(ZX.Drawing.IFactory))
                as ZX.Drawing.IFactory
                ?? throw new InvalidOperationException("Unable to get dependency ZX.Drawing.IFactory.");

            _platformFactory = dependencies.TryGetInstance("ZX.Platform.IFactory",
                typeof(ZX.Platform.IFactory))
                as ZX.Platform.IFactory
                ?? throw new InvalidOperationException("Unable to get dependency ZX.PLatform.IFactory.");

            _gameProvider = dependencies.TryGetInstance(Game.ClassNames.GameProvider,
                typeof(Game.IGameProvider))
                as Game.IGameProvider
                ?? throw new InvalidOperationException("Unable to get dependency ZX.Game.IGameProvider.");

            _flags = dependencies.TryGetInstance("ZX.Game.Flags",
				typeof(ZX.Game.IFlags))
				as ZX.Game.IFlags
				?? throw new NullReferenceException("Unable to get ZX.Game.IFlags dependency.");

		    _gameFactory = dependencies.TryGetInstance("ZX.Game.Factory",
				typeof(ZX.Game.IFactory))
				as ZX.Game.IFactory
				?? throw new NullReferenceException("Unable to get ZX.Game.IFactory dependency.");

            _bindingManager = dependencies.TryGetInstance<IBindingManager>(Bindings.ClassNames.BindingManager);

        }

        void IBuildable.EndBuild()
        {
            _bindingManager.Bind(BoundValueNames.RoomIndex, RoomIndexValueChanged);
           CreateFlags();  
           SetupWally();
           CreateAnimationLayer();

            _screen.AddLayer(_wallyController.Layer);
            _screen.AddLayer(_animationLayer as ILayer 
               ?? throw new InvalidOperationException(nameof(_animationLayer)));
            
            _gameProvider.AddItem(_wallyController);
            _gameProvider.AddItem(this);
            _gameProvider.AddItem(_roomProvider);
            _gameProvider.AddItem(_inventoryController);
            _gameProvider.AddItem(_animationLayer as IGameStatic 
                ?? throw new InvalidOperationException("Animation layer should be based on IGameStatic"));
        }

        #endregion

        #region Private helpers
        private void RoomIndexValueChanged(string name,  Type  type, object? value)
        {
            if(name == BoundValueNames.RoomIndex &&
                value is not null)
            { 
                _roomIndex = (int)value;
            }
        }
        
        private void CreateFlags()
        {
            _flags.RegisterFlag(FlagsNames.LiftCount, _gameFactory.CreateFlag(1, -1));
            _flags.RegisterFlag(FlagsNames.LiftFloor, _gameFactory.CreateFlag(4, -1));
            _flags.RegisterFlag(FlagsNames.HelpSwitch, _gameFactory.CreateFlag(0, -1));
            _flags.RegisterFlag(FlagsNames.LaserGun, _gameFactory.CreateFlag(0, 0x0d));
            _flags.RegisterFlag(FlagsNames.Bucket, _gameFactory.CreateFlag(0, 0x10));
            _flags.RegisterFlag(FlagsNames.FuelCan, _gameFactory.CreateFlag(0, 0x08));
            _flags.RegisterFlag(FlagsNames.MagLockDir, _gameFactory.CreateFlag(0, -1));
            _flags.RegisterFlag(FlagsNames.ArcadeMode, _gameFactory.CreateFlag(0, -1));
        }

        private void SetupWally()
        {
            const int ww = 16;
            const int wh = 16;

            ISurface surface = _platformFactory.CreateSurface();
            IDrawer drawer = _drawFactory.CreateBitmapDrawer(MemoryChunkNames.WallyBitmaps, ww, wh);
            surface.Create(ww, wh*2);
            
            Wally.DrawLayer layer = new Wally.DrawLayer(drawer, surface, (int)LayerZOrders.Wally)
            {
                X = 64,
                Y = 100
            };

            _wallyController.Layer = layer;
        }

        private void CreateAnimationLayer()
        {
            ISurface surface = _platformFactory.CreateSurface();
            IDrawer drawer = _drawFactory.CreateBitmapDrawer(MemoryChunkNames.AnimationBitmaps, 0x10, 0x10);

            surface.Create(Hardware.ScreenWidth, Hardware.ScreenHeight);
            _animationLayer = _drawFactory.CreateAnimationLayer("Animation", drawer, surface, (int)LayerZOrders.Animation);
        }
        #endregion

        #region IGameItem

        void IGameItem.Update()
        {
            _actionController.CheckActions(_roomIndex);
        }

        void IGameStatic.NewGame()
        {

        }

        void IGameStatic.NewLevel()
        {
   
        }

        #endregion

    }
}