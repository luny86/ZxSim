using Godot;
using System;
using KUtil;
using KUi;

public partial class TileGridControl : TextureRect
{	
	private TileGrid _tileGrid;

	public TileGridControl()
	: base()
	{
		_tileGrid = CPU.Instance.CreateTileGrid();
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var tiles = GetParent().GetNode<TextureRect>("TilesTextureRect");
		//tiles.SelectTile += OnSelectTile;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnSelectTile(int index)
	{
		Image image = _tileGrid.Draw(index);
		var texture = ImageTexture.CreateFromImage(image);
		this.Texture = texture;
	}
}
