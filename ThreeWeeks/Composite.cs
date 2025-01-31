
using GameEditorLib.Builder;
using GameEditorLib.Platform;
using KUi;
using ZX.Util;
using System.Collections.Generic;

namespace ThreeWeeks;

public class Composite : IBuildable, IComposition
{
    #region Memory Map
    // True address for start of ram in buffer.
    private const int _startAddr = 0x4000;

	// Tile bitmaps
	private Chunk _tileBmpChunk = null!;
	// Furniture strings
	private Chunk _furnitureStrings = null!;
	// Furniture table
	private Chunk _furnitureStringTable = null!;

	// Table of background attributes for each room.
	private Chunk _roomAttrTable = null!;

	// Room data
	private Chunk _roomData = null!;
    #endregion

    #region Members
    private byte[] _ram = null!;

    private FurnitureDraw _furnitureDraw = null!;

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
            view.Surface,
            new FurnitureDrawer(
					_furnitureStrings,
					_furnitureStringTable,
					_tileBmpChunk),
                    // Number of items in table.
                    _furnitureStringTable.Length/2);
        _furnitureDraw.Draw();
    }

    private void CreateMemoryMaps()
    {
		_tileBmpChunk = new Chunk("Tiles", 0x7da5, 0x1de6, _ram);
		_furnitureStrings = new Chunk("Furniture", 0x6c13, 0x1fe5, _ram);
		_furnitureStringTable = new Chunk("Furniture Table", 0x7c87, 0x11e, _ram);
		_roomAttrTable = new Chunk("Room Attrs", 0xd170, 0x20, _ram);
		_roomData = new Chunk("Rooms", 0xc977, 0x1000, _ram);
    }
    #endregion
}
