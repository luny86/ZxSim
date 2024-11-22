using Godot;
using KUi;
using Platform;
using System;
using GameEditorLib.Platform;

public partial class TileGridNode : Sprite2D
{
	private TileGrid _tileGrid;

	public TileGridNode()
	: base()
	{
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// TODO - Factory
		ISurface surface = new Factory().CreateSurface();
		surface.Updated += Surface_Updated;
		_tileGrid = CPU.Instance.CreateTileGrid(surface);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SelectTile(int index)
	{
		_tileGrid.Draw(index);
	}

	private void Surface_Updated(object sender, EventArgs e)
	{
		if(sender is Surface surface)
		{
			Texture = ImageTexture.CreateFromImage(surface.Image);
		}
	}
}
