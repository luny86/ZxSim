using Godot;
using KUtil;
using KUi;
using Platform;

public partial class CPU : Node
{
	// True address for start of ram in buffer.
	private int _startAddr = 0x4000;
	
	public static CPU Instance { get; private set; }
	
	private byte[] _ram;

	// Tile bitmaps
	private Chunk _tileChunk;
	// Furniture strings
	private Chunk _tileStrings;
	// Furniture table
	private Chunk _tileStringTable;

	// Table of background attributes for each room.
	private Chunk _roomAttrTable;

	// Room data
	private Chunk _roomData;

	public Tiles CreateTiles(ISurface surface)
	{
		return new Tiles(_tileChunk, surface);
	}

	public TileGrid CreateTileGrid(ISurface surface)
	{
		return new TileGrid(_tileChunk, surface);
	}

	public FurnitureDraw CreateFurnitureDraw(ISurface surface)
	{
		return new FurnitureDraw(_tileStrings, _tileStringTable,
			new Chunk(0x4000, 0xbfff, _ram), surface);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		using var file = FileAccess.Open("res://game.bin", FileAccess.ModeFlags.Read);
		_ram = file.GetBuffer(49152);

		_tileChunk = new Chunk(0x7da5, 0x1de6, _ram);
		_tileStrings = new Chunk(0x6c13, 0x1fe5, _ram);
		_tileStringTable = new Chunk(0x7c87, 0x11e, _ram);
		_roomAttrTable = new Chunk(0xd170, 0x20, _ram);
		_roomData = new Chunk(0xc977, 0x200, _ram);
		
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
