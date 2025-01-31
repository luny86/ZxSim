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

public partial class Main : Node, IBuildable
{
	private static Main _singleton = null!;

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
			Creator creator = new Creator();
			creator.BuildAll(this);		
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
	void IBuildable.AskForDependents(IRequests requests)
	{
		requests.AddRequest("ZX.Drawing.IFactory", 
				typeof(ZX.Drawing.IFactory));
	}

	void IBuildable.RegisterObjects(IDependencyPool dependencies)
	{
		using var file = FileAccess.Open("res://pyjamarama.bin", FileAccess.ModeFlags.Read);

		byte[] ram = file.GetBuffer(0x10000);
		_map = new MemoryMap(0x4000, ram);
		_map.AddRange("Tiles", 0xc1a3, 0x1158);

		dependencies.Add("Platform.Main.IView", 
			typeof(IView),
			_view);
		dependencies.Add("Platform.Main.IMemoryMap",
			typeof(ZX.Util.IMemoryMap),
			_map);
	}

	void IBuildable.DependentsMet(IDependencies dependencies)
	{
		object? temp = 
			dependencies.TryGetInstance(
				"ZX.Drawing.IFactory", 
				typeof(ZX.Drawing.IFactory));
		ZX.Drawing.IFactory factory = temp
			as ZX.Drawing.IFactory;

		
		ZX.Drawing.IDrawer drawer = factory.CreateTileDrawer(_map["Tiles"]);
		drawer.Draw(_view.Surface, 4, 10,10);

	}
	#endregion
}
