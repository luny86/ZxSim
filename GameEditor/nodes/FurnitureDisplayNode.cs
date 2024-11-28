using Godot;
using System;
using KUi;
using GameEditorLib.Ui;
using GameEditorLib.Platform;
using Platform;

public partial class FurnitureDisplayNode : TextureRect, IView
{
	private ISurface _surface;

	ISurface IView.Surface
	{
		get
		{
			return _surface;
		}

		set
		{
			if(_surface != null)
			{
				_surface.Updated -= Surface_Updated;
			}

			_surface = value;
			_surface.Updated += Surface_Updated;
		}
	}

	public override void _Ready()
	{
		//IFactory factory = new Factory();
		// TODO - Factory
		//GameEditorLib.Platform.ISurface surface = factory.CreateSurface();
		//surface.Updated += Surface_Updated;
		//_furniture = CPU.Instance.CreateFurnitureDraw(surface);
		//_furniture.Draw();
		
		/*
		BaseButton button = GetParent().GetNode<BaseButton>("NextItemButton");
		
		if(button is not null)
		{
			button.Pressed += NextButton_Pressed;
		}

		button = GetParent().GetNode<BaseButton>("PrevItemButton");

		if(button is not null)
		{
			button.Pressed += PrevButton_Pressed;
		}
		*/
	}

	private void NextButton_Pressed()
	{
		//_furniture.NextItem();
	}

	private void PrevButton_Pressed()
	{
		//_furniture.PreviousItem();
	}
	
	private void Surface_Updated(object sender, EventArgs e)
	{
		Godot.GD.Print("Updated");
		if(sender is Surface surface)
		{
			Texture = ImageTexture.CreateFromImage(surface.Image);
		}
	}
}
