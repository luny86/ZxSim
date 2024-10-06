using Godot;
using System;
using KUtil;
using KUi;

public partial class TextureRect : Godot.TextureRect
{
	[Signal]
	public delegate void SelectTileEventHandler(int index);

	private Tiles _tiles;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tiles = CPU.Instance.CreateTiles();
		Image image = _tiles.Draw();
		var texture = ImageTexture.CreateFromImage(image);
		this.Texture = texture;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _GuiInput(InputEvent input)
	{
		TextEdit node = GetParent().GetNode("TextEdit") as TextEdit;
		
		if(input is InputEventMouseButton mouse)
		{
			if(mouse.Pressed)
			{
				int tileSelected = _tiles.PointToTile(mouse.Position);
				//EmitSignal(SignalName.SelectTile, tileSelected);
				node.Text = "Clicked";
			}
		}
		else if(input is InputEventMouseMotion motion)
		{
			//node.Text = _tiles.PointToTile(motion.Position).ToString();
			
		}
	}
}
