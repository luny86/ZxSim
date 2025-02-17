using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using Platform;
using ZX.Platform;
using Builder;
using ZX;
using ZX.Util;
using ZX.Drawing;
using ZX.Game;
using Pyjamarama;

public partial class Main : Node, IBuildable
{


	private static Main _singleton = null!;

	Pyjamarama.IFactory _factory = null;

	ZX.Platform.IFactory _platformFactory = null;

	ZX.Drawing.IScreen _screen =  null;
	
	private UserInputBridge _input;

	private PackedScene _mainScene;
	private IView _view;

	private MemoryMap _map;


	public static Main Singleton { get { return _singleton;} }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainScene = GD.Load<PackedScene>("res://view.tscn");
		_singleton = this;
		_view = CreateScreen();

		try
		{
			Creator creator = new Creator(true);
			creator.BuildAll(this);		
			GD.Print(creator);

			CreateLayers();
			// Test
			ILayer layer = _screen["background"];
			layer.Update();
		}
		catch (InvalidOperationException e)
		{
			GD.Print($"Error {e.Message}");
			GD.Print(e);
		}
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
		
	public IView CreateScreen()
	{
		Node inst = _mainScene.Instantiate();
		AddChild(inst);
		View view = inst as View;

		if(view is not IView)
		{
			throw new InvalidOperationException($"Node should implment IView. Error when trying to create 'view'.");
		}

		return view;
	}

	#region IBuildables
	IList<IBuildable> IBuildable.CreateBuildables()
	{
		IBuildable view = _view as IBuildable ?? throw new InvalidOperationException("View must be an IBuildable");

		return new List<IBuildable>()
		{
			view
		};
	}

	void IBuildable.AskForDependents(IRequests requests)
	{
		requests.AddRequest("Pyjamarama.Factory", 
				typeof(Pyjamarama.IFactory));
		requests.AddRequest("ZX.Platform.IFactory",
				typeof(ZX.Platform.IFactory));
		requests.AddRequest("ZX.Drawing.Screen",
				typeof(ZX.Drawing.IScreen));
		requests.AddRequest("ZX.Platform.UserInput",
				typeof(ZX.Platform.IUserInput));
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
		
		_input =
			dependencies.TryGetInstance(
				"ZX.Platform.UserInput",
				typeof(ZX.Platform.IUserInput))
			as UserInputBridge
			?? throw new InvalidOperationException("Unable to get dependency ZX.Platform.UserInput");

		ISurface surface = _platformFactory.CreateSurface();
		surface.Create(256, 192);
		surface.Fill(ZX.Palette.Green);
		_view.Surface = surface;
	}
	
	void IBuildable.EndBuild()
	{
		
	}
	#endregion
	
	public override void _Input(InputEvent e)
	{
		_input?.HandleInput(e);
	}
}
