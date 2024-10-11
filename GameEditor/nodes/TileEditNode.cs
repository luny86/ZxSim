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
	public class SelectTileEventArgs : EventArgs
	{
		public SelectTileEventArgs(int index)
		{
			Index = index;
		}
		
		public int Index { get; }
	}
	
	public event EventHandler<SelectTileEventArgs> SelectTile;
	
	private Tiles _tiles;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tiles = CPU.Instance.CreateTiles();
		Surface image = _tiles.Draw() as Surface;
		var texture = ImageTexture.CreateFromImage(image.Image);
		Texture = texture;
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
