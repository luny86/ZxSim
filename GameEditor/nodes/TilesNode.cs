using Godot;
using KUi;
using Platform;
using System;

public partial class TilesNode : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ISurface surface = Factory.CreateSurface();
		surface.Updated += Surface_Updated;
		Tiles tiles = CPU.Instance.CreateTiles(surface);
		tiles.Draw() ;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Surface_Updated(object sender, EventArgs e)
	{
		if(sender is Surface surface)
		{
			var texture = ImageTexture.CreateFromImage(surface.Image);
		}
	}
}
