using Godot;
using System;
using KUi;
using Platform;

public partial class FurnitureDisplayNode : TextureRect
{
	private FurnitureDraw _furniture;

	public override void _Ready()
	{
		ISurface surface = Factory.CreateSurface();
		surface.Updated += Surface_Updated;
		_furniture = CPU.Instance.CreateFurnitureDraw(surface);
		_furniture.Draw();
		
		BaseButton button = GetParent().GetNode<BaseButton>("NextItemButton");
		button.Pressed += NextButton_Pressed;
        button = GetParent().GetNode<BaseButton>("PrevItemButton");
        button.Pressed += PrevButton_Pressed;
	}

    private void NextButton_Pressed()
    {
        _furniture.NextItem();
    }

    private void PrevButton_Pressed()
    {
        _furniture.PreviousItem();
    }
	
	private void Surface_Updated(object sender, EventArgs e)
	{
		if(sender is Surface surface)
		{
			Texture = ImageTexture.CreateFromImage(surface.Image);
		}
	}
}
