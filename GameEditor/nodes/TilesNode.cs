using Godot;
using System;
using KUtil;
using KUi;

public partial class TilesNode : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Tiles tiles = CPU.Instance.CreateTiles();
		Image image = tiles.Draw();
		var texture = ImageTexture.CreateFromImage(image);
		this.Texture = texture;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
