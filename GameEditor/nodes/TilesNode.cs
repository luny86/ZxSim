using Godot;
using KUi;
using Platform;

public partial class TilesNode : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Tiles tiles = CPU.Instance.CreateTiles();
		Surface image = tiles.Draw() as Surface;
		var texture = ImageTexture.CreateFromImage(image.Image);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
