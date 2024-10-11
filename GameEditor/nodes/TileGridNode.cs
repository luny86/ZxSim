using Godot;
using KUi;
using Platform;

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
		_tileGrid = CPU.Instance.CreateTileGrid();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SelectTile(int index)
	{
		Surface image = _tileGrid.Draw(index) as Surface;
		var texture = ImageTexture.CreateFromImage(image.Image);
		Texture = texture;
	}
}
