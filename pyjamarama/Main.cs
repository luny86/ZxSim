using Godot;
using System;
using System.Collections.Generic;
using Platform;
using ZX.Platform;
using GameEditorLib.Builder;
using ZX;
using ZX.Util;

public partial class Main : Node, IComposition, IBuildable
{
	private static Main _singleton = null!;

	private PackedScene _commandScene;
	private IView _view;

	public static Main Singleton { get { return _singleton;} }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_commandScene = GD.Load<PackedScene>("res://view.tscn");
		_singleton = this;
		_view = CreateScreen();

		Creator creator = new Creator();
		creator.BuildAll();		
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
			surface.Fill(ZX.Palette.Black);
			viewer.Surface = surface;
		}
		else
		{
			throw new InvalidOperationException($"Node without IView when trying to create command 'view'");
		}

		return view;
	}

	#region IComposition
	string IComposition.Name => "Platform.Main";

	IList<IBuildable>? IComposition.CreateBuildables()
	{
		return null;
	}
	#endregion

	#region IBuildables
	void IBuildable.AskForDependents(IRequests requests)
	{
	}

	void IBuildable.RegisterObjects(IDependencyPool dependencies)
	{
		using var file = FileAccess.Open("res://pyjamarama.bin", FileAccess.ModeFlags.Read);

		byte[] ram = file.GetBuffer(0x10000);
		MemoryMap map = new MemoryMap(0x4000, ram);
		map.AddRange("Tiles", 0xc1a0, 0x1158);

		dependencies.Add("Platform.Main.IView", 
			typeof(IView),
			_view);
		dependencies.Add("Platform.Main.IMemoryMap",
			typeof(ZX.Util.IMemoryMap),
			map);
	}


	void IBuildable.DependentsMet(IDependencies dependencies)
	{
	}
	#endregion
}
