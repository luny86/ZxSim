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

	private Chunk _tileChunk;

	public Tiles CreateTiles(ISurface surface)
	{
		return new Tiles(_tileChunk, surface);
	}

	public TileGrid CreateTileGrid(ISurface surface)
	{
		return new TileGrid(_tileChunk, surface);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		using var file = FileAccess.Open("res://game.bin", FileAccess.ModeFlags.Read);
		_ram = file.GetBuffer(49152);

		_tileChunk = new Chunk(0x7da5, 0x1de6, _ram);
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
