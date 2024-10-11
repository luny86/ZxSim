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
	/// <summary>Invoked when a tile has been altered.</summary>
	public event  EventHandler<SelectTileEventArgs> TileChanged;

	private TileGrid _tileGrid;

	public TileGridControl()
	: base()
	{
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ISurface surface = Factory.CreateSurface();
		surface.Updated += Surface_Updated;
		_tileGrid = CPU.Instance.CreateTileGrid(surface);
		UpdateGrid(0);

		// Hook into the selection tool
		var tiles = GetParent().GetNode<TileEditNode>("TileSelect");

		tiles.SelectTile += OnSelectTile;
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
				_tileGrid.TogglePixel((int)mouse.Position.X, (int)mouse.Position.Y);
				OnTileChanged();
			}
		}
		else
		{
        	base._GuiInput(input);
		}
    }

    public void OnSelectTile(object sender, EventArgs e)
	{
		if(e is SelectTileEventArgs args)
		{
			UpdateGrid(args.Index);
		}
	}
	
	private void UpdateGrid(int index)
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

	private void OnTileChanged()
	{
		TileChanged?.Invoke(this, 
		new SelectTileEventArgs(_tileGrid.TileIndex));
	}
}
