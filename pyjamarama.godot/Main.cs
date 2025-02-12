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
using Pyjamarama;

public partial class Main : Node, IBuildable
{
	private static Main _singleton = null!;

	Pyjamarama.IFactory _factory = null;

	ZX.Platform.IFactory _platformFactory = null;

	ZX.Drawing.IScreen _screen =  null;

	private PackedScene _commandScene;
	private IView _view;

	private MemoryMap _map;

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

			IDrawer drawer = _factory.CreateRoomDrawer("RoomPointers", "Rooms", "Tiles", "Furniture");

			ISurface bg = _platformFactory.CreateSurface();
			
			_screen.Main = _view.Surface;

			BackgroundLayer layer = new BackgroundLayer(drawer, bg, 1);
			layer.Index = 1;
			layer.Update();
			_screen.AddLayer(layer);
			
			_screen.Update();
		}
		catch (InvalidOperationException e)
		{
			GD.Print($"Error {e.Message}");
		}
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

		GD.Print(_map["Furniture"][0]);
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
	}
	#endregion
}
