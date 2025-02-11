
using GameEditorLib.Builder;
using ZX.Drawing;

namespace Pyjamarama
{
    internal class Factory : IFactory, IBuildable
    {
        private ZX.Drawing.IFactory _factory = null!;
        private ZX.Util.IMemoryMap _map = null!;

        IDrawer IFactory.CreateRoomDrawer(string addressTableName, string dataChunkName, string tileChunkName, string furnitureChunkName)
        {
            ZX.Drawing.IDrawer drawer = _factory.CreateTileDrawer(tileChunkName);
            FurnitureDrawer furniture = new FurnitureDrawer(drawer, _map[furnitureChunkName]);
            return new RoomDrawer(furniture, _map[dataChunkName], _map[addressTableName]);
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