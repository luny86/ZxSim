
using GameEditorLib.Builder;
using GameEditorLib.Platform;
using KUi;
using KUtil;
using System.Collections.Generic;

namespace ThreeWeeks;

public class Composite : IBuildable, IComposition
{
    #region Memory Map
    // True address for start of ram in buffer.
    private const int _startAddr = 0x4000;

	// Tile bitmaps
	private Chunk _tileBmpChunk;
	// Furniture strings
	private Chunk _furnitureStrings;
	// Furniture table
	private Chunk _furnitureStringTable;

	// Table of background attributes for each room.
	private Chunk _roomAttrTable;

	// Room data
	private Chunk _roomData;
    #endregion

    #region Members
    private byte[] _ram;

    private FurnitureDraw _furnitureDraw;

    #endregion

    #region Construction
    public Composite()
    {
    }
    #endregion

    #region IComposition
    string IComposition.Name => "ThreeWeeks";

    IList<IBuildable> IComposition.CreateBuildables()
    {
        return null;
    }
    #endregion

    #region IBuildable
    void IBuildable.RegisterObjects(IDependencyPool dependencies)
    {
		// TODO - Node to deal surface.Updated += Surface_Updated;
		//KUi.FurnitureDraw furniture = CPU.Instance.CreateFurnitureDraw(surface);

        //dependencies.Add(typeof(KUi.FurnitureDrawer), furniture);
    }

    void IBuildable.AskForDependents(IRequests requests)
    {
        requests.AddRequest("GameEditorLib.Platform.IFactory", typeof(IFactory));
    }

    void IBuildable.DependentsMet(IDependencies dependencies)
    {
        IFactory factory = 
            dependencies.TryGetInstance("GameEditorLib.Platform.IFactory", typeof(IFactory))
            as IFactory;

        _ram = factory.LoadBinary("res://game.bin");
        CreateMemoryMaps();

        IView view = factory.CreateCommand("furniture");

        _furnitureDraw = new FurnitureDraw();
        _furnitureDraw.Initialise(
            _furnitureStrings, 
            _furnitureStringTable, 
            _tileBmpChunk,
            view.Surface);
        _furnitureDraw.Draw();
    }

    private void CreateMemoryMaps()
    {
		_tileBmpChunk = new Chunk(0x7da5, 0x1de6, _ram);
		_furnitureStrings = new Chunk(0x6c13, 0x1fe5, _ram);
		_furnitureStringTable = new Chunk(0x7c87, 0x11e, _ram);
		_roomAttrTable = new Chunk(0xd170, 0x20, _ram);
		_roomData = new Chunk(0xc977, 0x1000, _ram);
    }
    #endregion
}
