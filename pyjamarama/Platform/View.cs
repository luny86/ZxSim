using Godot;
using System;
using GameEditorLib.Platform;
using Platform;

public partial class View : TextureRect, IView
{
	private ISurface _surface;

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

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		_surface.BeginDraw();
		_surface.EndDraw();
	}	

	private void Surface_Updated(object sender, EventArgs e)
	{
		if(sender is Surface surface)
		{
			Texture = ImageTexture.CreateFromImage(surface.Image);
		}
	}
}
