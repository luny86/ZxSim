using Godot;
using System;
using KUi;
using Platform;

/// <summary>
/// Control that displays a tile in a grid
/// and allows user to edit.
/// </summary>
public partial class TileGridControl : TextureRect
{	
	private TileGrid _tileGrid;

	public TileGridControl()
	: base()
	{
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tileGrid = CPU.Instance.CreateTileGrid();
		UpdateGrid(0);
		var tiles = GetParent().GetNode<TileEditNode>("TileSelect");

		tiles.SelectTile += OnSelectTile;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnSelectTile(object sender, EventArgs e)
	{
		if(e is TileEditNode.SelectTileEventArgs args)
		{
			UpdateGrid(args.Index);
		}
	}
	
	private void UpdateGrid(int index)
	{
		// change to an update event to pass back 'image'
		Surface image = _tileGrid.Draw(index) as Surface;
		var texture = ImageTexture.CreateFromImage(image.Image);
		Texture = texture;
	}
}
