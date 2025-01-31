using System;
using Godot;
using ZX.Util;
using KUi;
using GameEditorLib.Ui;
using GameEditorLib.Platform;

public partial class CPU : Node
{
	// 
	// Info to move into ZX
	//
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

	private PackedScene _commandScene;

	public Tiles CreateTiles(ISurface surface)
	{
		return new Tiles(_tileChunk, surface);
	}

	public TileGrid CreateTileGrid(ISurface surface)
	{
		return new TileGrid(_tileChunk, surface);
	}

	public RoomDraw CreateRoomDraw(ISurface surface)
	{
		return new RoomDraw(_roomAttrTable,
			_roomData,
			CreateFurnitureDrawer() as IFurnitureDrawer,
			surface);
	}

	private IDrawer CreateFurnitureDrawer()
	{
		return new ThreeWeeks.FurnitureDrawer(_tileStrings, _tileStringTable,
			new Chunk("Full Ram", 0x4000, 0xbfff, _ram));
	}

	public IView CreateCommand(string name)
	{
		Node inst = _commandScene.Instantiate();
		AddChild(inst);	
		FurnitureDisplayNode view = inst.GetNode<FurnitureDisplayNode>("Panel/Window/View");

		if(view is not IView)
		{
			throw new InvalidOperationException($"Node without IView when trying to create command '{name}'");
		}

		return view;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;		
		_commandScene = GD.Load<PackedScene>("res://command.tscn");

		//
		//
		//
		GameEditorLib.GameEditor.Initialise();
		//
		//

		/*
		//
		// Platform specific
		// IMemory { filename, ram }		
		using var file = FileAccess.Open("res://game.bin", FileAccess.ModeFlags.Read);
		_ram = file.GetBuffer(49152);

		// 
		// Move into Three Weeks composite.
		_tileChunk = new Chunk(0x7da5, 0x1de6, _ram);
		_tileStrings = new Chunk(0x6c13, 0x1fe5, _ram);
		_tileStringTable = new Chunk(0x7c87, 0x11e, _ram);
		_roomAttrTable = new Chunk(0xd170, 0x20, _ram);
		_roomData = new Chunk(0xc977, 0x1000, _ram);
		*/
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
