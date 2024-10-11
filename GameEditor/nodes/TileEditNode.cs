using Godot;
using System;
using KUi;
using Platform;

/// <summary>
/// Node which displays all tiles and allows
/// user to select one.
/// </summary>
public partial class TileEditNode : TextureRect
{
	public event EventHandler<SelectTileEventArgs> SelectTile;
	
	private Tiles _tiles;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ISurface surface = Factory.CreateSurface();
		surface.Updated += Surface_Updated;
		_tiles = CPU.Instance.CreateTiles(surface);
		_tiles.Draw();

		var tileGrid = GetParent().GetNode<TileGridControl>("TileGrid");
		tileGrid.TileChanged += TileGrid_TileChanged;
	}

	private void TileGrid_TileChanged(object sender, EventArgs e)
	{
		if(e is SelectTileEventArgs select)
		{
			_tiles.DrawTile(select.Index);		
		}
	}

	private void Surface_Updated(object sender, EventArgs e)
	{
		if(sender is Surface surface)
		{
			Texture = ImageTexture.CreateFromImage(surface.Image);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _GuiInput(InputEvent input)
	{
		if(input is InputEventMouseButton mouse)
		{
			if(mouse.Pressed)
			{
				int tileSelected = 
					_tiles.PointToTile((int)mouse.Position.X, (int)mouse.Position.Y);
				TileSelect(tileSelected);
			}
		}
	}
	
	private void TileSelect(int index)
	{
		SelectTile?.Invoke(this, new SelectTileEventArgs(index));
	}
}
