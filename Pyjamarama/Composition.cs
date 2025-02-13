
using GameEditorLib.Builder;
using System.Runtime.CompilerServices;
using ZX.Drawing;
using ZX.Platform;
using Wally = Pyjamarama.Wally;


[assembly:InternalsVisibleTo("PyjamaramaTests")]

namespace Pyjamarama
{
    internal class Composition : IComposition, IBuildable
    {

        private Factory _factory = new Factory();

        private Wally.Controller _wallyController = new Wally.Controller();

        ZX.Drawing.IFactory _drawFactory = null!;

        ZX.Platform.IFactory _platformFactory = null!;

        IScreen _screen = null!;

        string IComposition.Name => "Pyjamarama";

        void IBuildable.AskForDependents(IRequests requests)
        {
            requests.AddRequest("ZX.Drawing.Screen", typeof(ZX.Drawing.IScreen));
            requests.AddRequest("ZX.Drawing.IFactory", typeof(ZX.Drawing.IFactory));
		    requests.AddRequest("ZX.Platform.IFactory", typeof(ZX.Platform.IFactory));
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
        }

        void IBuildable.EndBuild()
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
                Y = 148
            };

            layer.Update();

            _screen.AddLayer(layer);
        }

    }
}