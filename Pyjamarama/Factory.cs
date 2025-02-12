
using GameEditorLib.Builder;
using ZX.Drawing;

namespace Pyjamarama
{
    internal class Factory : IFactory, IBuildable
    {
        private ZX.Drawing.IFactory _factory = null!;
        private ZX.Util.IMemoryMap _map = null!;
        private ZX.Game.IFlags _flags = null!;

        IDrawer IFactory.CreateRoomDrawer(string addressTableName, string dataChunkName, string tileChunkName, string furnitureChunkName)
        {
            ZX.Drawing.IDrawer drawer = _factory.CreateTileDrawer(tileChunkName);
            FurnitureDrawer furniture = new FurnitureDrawer(drawer, _map[furnitureChunkName]);

            drawer = _factory.CreateTileDrawer(MemoryChunkNames.WallTileBitmaps);
            WallDrawer walls = new WallDrawer(drawer, _map[MemoryChunkNames.WallTileBitmaps]);

            return new RoomDrawer(furniture,walls,  _map[dataChunkName], _map[addressTableName], _flags);
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
            requests.AddRequest("ZX.Game.Flags",
                typeof(ZX.Game.IFlags));
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
                    ?? throw new NullReferenceException("Unable to get ZX.Util.IMemoryMap dependency. This needs to be created by the main project.");

            _flags = dependencies.TryGetInstance(
                        "ZX.Game.Flags",
                        typeof(ZX.Game.IFlags))
                    as ZX.Game.IFlags
                    ?? throw new NullReferenceException("Unable to get ZX.Game.IFlags dependency.");
        }
    }
}