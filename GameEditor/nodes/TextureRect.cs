using Godot;
using System;
using KUtil;
using KUi;

public partial class TextureRect : Godot.TextureRect
{
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
				node.Text = "Clicked";
			}
		}
		else if(input is InputEventMouseMotion motion)
		{
			node.Text = motion.Position.ToString();
			
		}
	}
}
