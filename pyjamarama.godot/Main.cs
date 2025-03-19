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
using Logging;

public partial class Main : Node, IBuildable
{


	private static Main _singleton = null!;

	Pyjamarama.IFactory _factory = null;

	ZX.Platform.IFactory _platformFactory = null;

	ILogger _logger = null;

	ZX.Drawing.IScreen _screen =  null;
	
	private UserInputBridge _input;

	private PackedScene _mainScene;
	private IView _view;

	private MemoryMap _map;

	private IAttributeTable _attributes;


	public static Main Singleton { get { return _singleton;} }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainScene = GD.Load<PackedScene>("res://view.tscn");
		_singleton = this;
		_view = CreateView();

		try
		{
			Creator creator = new Creator(true);
			creator.BuildAll(this);		
			GD.Print(creator);

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
		
	public IView CreateView()
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
		requests.AddRequest(Logging.ClassNames.Factory,
				typeof(Logging.IFactory));
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
		_map.AddRange(MemoryChunkNames.WallyBitmaps, 0x91eb, 0x1900);
		_map.AddRange(MemoryChunkNames.ObjectBitmaps, 0x9beb, 0xf00);
		_map.AddRange(MemoryChunkNames.InventoryFurniture, 0x826c, 0x5a);
		_map.AddRange(MemoryChunkNames.LivesBitmaps, 0x9b6b, 0x80);
		_map.AddRange(MemoryChunkNames.MilkGlass, 0x8302, 0xc0);
		_map.AddRange(MemoryChunkNames.ObjectTextTable, 0xe48e, 0x40);
		_map.AddRange(MemoryChunkNames.ObjectText, 0xe27d, 0x211);
		_map.AddRange(MemoryChunkNames.AnimationBitmaps, 0x9beb, 0xf00);

		dependencies.Add("Platform.Main.IView", 
			typeof(IView),
			_view);
		dependencies.Add(ZX.Platform.ClassNames.MemoryMap,
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

		Logging.IFactory logFactory = dependencies.TryGetInstance<Logging.IFactory>(Logging.ClassNames.Factory);
		_logger = logFactory.GetLogger();

		ISurface surface = _platformFactory.CreateSurface();
		surface.Create(256, 192);
		surface.Fill(ZX.Palette.Green);
		_view.Surface = surface;
	}
	
	void IBuildable.EndBuild()
	{
		_attributes = _factory.GetAttributeTable();
	}
	#endregion
	
	public override void _Input(InputEvent e)
	{
		_input?.HandleInput(e);

		if(e.IsActionPressed("dump"))
		{
			_logger.WriteLog(LogLevel.Info, "Main", _attributes.ToString());
		}
	}
}
