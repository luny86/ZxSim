using Godot;
using System;
using System.Collections.Generic;
using Builder;
using ZX.Platform;
using Platform;
using Game = ZX.Game;
using Drawing = ZX.Drawing;

public partial class View : TextureRect, IView, IBuildable
{
	#region Members

	private bool _ready = false;

	private Game.IGameProvider _gameProvider = null!;


	private Drawing.IScreen _screen =  null;

	private ISurface _surface;

	#endregion 

	#region Properties

	ISurface IView.Surface
	{
		get
		{
			return _surface;
		}

		set
		{
			if(_surface != null)
			{
				_surface.Updated -= Surface_Updated;
			}

			_surface = value;
			_surface.Updated += Surface_Updated;
		}
	}

	#endregion

	#region IBuildable
	void IBuildable.AskForDependents(IRequests requests)
	{
		requests.AddRequest(Game.ClassNames.GameProvider, typeof(Game.IGameProvider));
		requests.AddRequest(Drawing.ClassNames.Screen, typeof(Drawing.IScreen));
	}

	IList<IBuildable>? IBuildable.CreateBuildables()
	{
		return null;
	}

	void IBuildable.RegisterObjects(IDependencyPool dependencies)
	{
		
	}

	void IBuildable.DependentsMet(IDependencies dependencies)
	{
		_gameProvider = dependencies.TryGetInstance(Game.ClassNames.GameProvider,
			typeof(Game.IGameProvider))
			as Game.IGameProvider
			?? throw new InvalidOperationException("Unable to get dependency ZX.Game.IGameProvider.");

		_screen = dependencies.TryGetInstance(Drawing.ClassNames.Screen,
			typeof(ZX.Drawing.IScreen))
			as ZX.Drawing.IScreen
			?? throw new InvalidOperationException("Unable to get dependency ZX.Drawing.IScreen.");

		_screen.Main = (this as IView).Surface;
	}

	void IBuildable.EndBuild()
	{
		_ready = true;
		Console.WriteLine("READY");
	}

	#endregion

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if(_ready)
		{
			_surface.BeginDraw();
			_gameProvider.Update();
			_screen.Update();
			_surface.EndDraw();
		}
	}	

	private void Surface_Updated(object sender, EventArgs e)
	{
		if(sender is Surface surface)
		{
			Texture = ImageTexture.CreateFromImage(surface.Image);
		}
	}
}
