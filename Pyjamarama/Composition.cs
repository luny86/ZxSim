
using Builder;
using System.Runtime.CompilerServices;
using ZX.Drawing;
using ZX.Platform;
using Game = ZX.Game;


[assembly:InternalsVisibleTo("PyjamaramaTests")]

namespace Pyjamarama
{
    internal class Composition : IComposition, IBuildable
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

        private Factory _factory = new Factory();

        private Wally.Controller _wallyController = new Wally.Controller();

        ZX.Drawing.IFactory _drawFactory = null!;

        ZX.Platform.IFactory _platformFactory = null!;

        private ZX.Game.IFactory _gameFactory = null!;

        IScreen _screen = null!;

        Game.IGameProvider _gameProvider = null!;

	    private ZX.Game.IFlags _flags = null!;

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
        }

        IList<IBuildable>? IBuildable.CreateBuildables()
        {
            return new List<IBuildable>()
            {
                _factory
            };
        }

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
            dependencies.Add("Pyjamarama.Factory", typeof(IFactory), _factory);
            dependencies.Add("Pyjamarama.Wally", typeof(Wally.Controller), _wallyController);
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
        }

        void IBuildable.EndBuild()
        {
           CreateFlags();
           
           Wally.Controller controller = CreateWally();

            _screen.AddLayer(controller.Layer);
            _gameProvider.AddItem(controller);
        }
    
        #endregion

        #region Private helpers
        
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

        private Wally.Controller CreateWally()
        {
           const int ww = 16;
            const int wh = 32;

            ISurface surface = _platformFactory.CreateSurface();
            IDrawer drawer = _drawFactory.CreateBitmapDrawer(MemoryChunkNames.WallyBitmaps, ww, wh);
            surface.Create(ww, wh);
            surface.Fill(ZX.Palette.BrightMagenta);
            
            Wally.DrawLayer layer = new Wally.DrawLayer(drawer, surface, 3)
            {
                X = 64,
                Y = 150
            };

            Wally.Controller controller = new Wally.Controller()
            {
                Layer = layer
            };

            return controller;
        }
        #endregion
    }
}