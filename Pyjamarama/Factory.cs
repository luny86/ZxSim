
using GameEditorLib.Builder;
using ZX.Util;

namespace Pyjamarama
{
    internal class Factory : IFactory, IBuildable
    {
        private ZX.Drawing.IFactory _factory = null!;
        private ZX.Util.IMemoryMap _map = null!;

        FurnitureDrawer IFactory.CreateFurnitureDrawer(string tileChunkName, string furnitureChunkName)
        {
            ZX.Drawing.IDrawer drawer = _factory.CreateTileDrawer(tileChunkName);
            return new Pyjamarama.FurnitureDrawer(drawer, _map[furnitureChunkName]);
        }

        IList<IBuildable>? IBuildable.CreateBuildables()
        {
            return new List<IBuildable>();
        }

        void IBuildable.RegisterObjects(IDependencyPool dependencies)
        {
        }

        void IBuildable.AskForDependents(IRequests requests)
        {
            requests.AddRequest("ZX.Drawing.IFactory", 
                typeof(ZX.Drawing.IFactory));
            requests.AddRequest("Platform.Main.IMemoryMap",
                typeof(ZX.Util.IMemoryMap));
        }


        void IBuildable.DependentsMet(IDependencies dependencies)
        {
    		_factory = dependencies.TryGetInstance(
                        "ZX.Drawing.IFactory", 
                        typeof(ZX.Drawing.IFactory))
            		as ZX.Drawing.IFactory
                    ?? throw new NullReferenceException("Unable to get ZX.Drawing.IFactory dependency.");

            _map = dependencies.TryGetInstance(
                        "Platform.Main.IMemoryMap",
                        typeof(ZX.Util.IMemoryMap))
                    as ZX.Util.IMemoryMap
                    ?? throw new NullReferenceException("Unable to get ZX.Util.ImemoryMap dependency.");
        }
    }
}