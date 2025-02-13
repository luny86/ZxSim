using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using Platform;
using ZX.Platform;
using GameEditorLib.Builder;
using ZX;
using ZX.Util;
using ZX.Drawing;
using ZX.Game;
using Pyjamarama;

public partial class Main : Node, IBuildable
{
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

	private static Main _singleton = null!;

	Pyjamarama.IFactory _factory = null;

	ZX.Platform.IFactory _platformFactory = null;

	ZX.Drawing.IScreen _screen =  null;

	private PackedScene _commandScene;
	private IView _view;

	private MemoryMap _map;

	private ZX.Game.IFlags _flags;
	private ZX.Game.IFactory _gameFactory;


	public static Main Singleton { get { return _singleton;} }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_commandScene = GD.Load<PackedScene>("res://view.tscn");
		_singleton = this;
		_view = CreateScreen();

		try
		{
			Creator creator = new Creator(true);
			creator.BuildAll(this);		
			GD.Print(creator);

			
			ISurface bg = _platformFactory.CreateSurface();
			
			_screen.Main = _view.Surface;
			
			CreateFlags();
			CreateLayers();
			// Test
			ILayer layer = _screen["background"];
			layer.Update();

			_screen.Update();
		}
		catch (InvalidOperationException e)
		{
			GD.Print($"Error {e.Message}");
			GD.Print(e);
		}
	}

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

	private void CreateLayers()
	{
		IDrawer drawer = _factory.CreateRoomDrawer("RoomPointers", "Rooms", "Tiles", "Furniture");
		ISurface bg = _platformFactory.CreateSurface();

		BackgroundLayer layer = new BackgroundLayer(drawer, "background", bg, 1)
		{
			RoomIndex = 4
		};

		_screen.AddLayer(layer);	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
		
	public IView CreateScreen()
	{
		Node inst = _commandScene.Instantiate();
		AddChild(inst);
		View view = inst as View;

		if(view is IView viewer)
		{
			Surface surface = new Surface();
			surface.Create(256, 192);
			surface.Fill(ZX.Palette.Green);
			viewer.Surface = surface;
		}
		else
		{
			throw new InvalidOperationException($"Node without IView when trying to create command 'view'");
		}

		return view;
	}

	#region IBuildables
	IList<IBuildable> IBuildable.CreateBuildables()
	{
		return null;
	}

	void IBuildable.AskForDependents(IRequests requests)
	{
		requests.AddRequest("Pyjamarama.Factory", 
				typeof(Pyjamarama.IFactory));
		requests.AddRequest("ZX.Platform.IFactory",
				typeof(ZX.Platform.IFactory));
		requests.AddRequest("ZX.Drawing.Screen",
				typeof(ZX.Drawing.IScreen));
		requests.AddRequest("ZX.Game.Flags",
				typeof(ZX.Game.IFlags));
		requests.AddRequest("ZX.Game.Factory",
				typeof(ZX.Game.IFactory));
	}

	void IBuildable.RegisterObjects(IDependencyPool dependencies)
	{
		using var file = FileAccess.Open("res://pyjamarama.bin", FileAccess.ModeFlags.Read);

		byte[] ram = file.GetBuffer(0x10000);
		_map = new MemoryMap(0x4000, ram);
		_map.AddRange(MemoryChunkNames.TileBitmaps, 0xc1a0, 0x1158);
		_map.AddRange(MemoryChunkNames.FurnitureStrings, 0xd2f8, 0xeac);
		_map.AddRange(MemoryChunkNames.RoomStrings, 0x8d6a, 0x301);
		_map.AddRange(MemoryChunkNames.RoomPointers, 0x8d2e, 0x3c);
		_map.AddRange(MemoryChunkNames.WallTileBitmaps, 0x8c4d, 0x18);
		_map.AddRange(MemoryChunkNames.WallyBitmaps, 0x91eb, 0xa00);

		dependencies.Add("Platform.Main.IView", 
			typeof(IView),
			_view);
		dependencies.Add("Platform.Main.IMemoryMap",
			typeof(ZX.Util.IMemoryMap),
			_map);
	}

	void IBuildable.DependentsMet(IDependencies dependencies)
	{
		_factory = 
			dependencies.TryGetInstance(
				"Pyjamarama.Factory", 
				typeof(Pyjamarama.IFactory))
		as Pyjamarama.IFactory;

		_platformFactory =
			dependencies.TryGetInstance(
				"ZX.Platform.IFactory",
				typeof(ZX.Platform.IFactory))
		as ZX.Platform.IFactory;

		_screen = 
			dependencies.TryGetInstance(
				"ZX.Drawing.Screen",
				typeof(ZX.Drawing.IScreen))
		as ZX.Drawing.IScreen;

		_flags = dependencies.TryGetInstance(
					"ZX.Game.Flags",
					typeof(ZX.Game.IFlags))
				as ZX.Game.IFlags
				?? throw new NullReferenceException("Unable to get ZX.Game.IFlags dependency.");

		_gameFactory = dependencies.TryGetInstance(
					"ZX.Game.Factory",
					typeof(ZX.Game.IFactory))
				as ZX.Game.IFactory
				?? throw new NullReferenceException("Unable to get ZX.Game.IFactory dependency.");
	}
	
	void IBuildable.EndBuild()
	{
		
	}
	#endregion
}
