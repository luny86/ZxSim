using Godot;
using System;
using KUi;
using Platform;
using GameEditorLib.Platform;

public partial class RoomDisplayNode : TextureRect
{
	private RoomDraw _roomDraw;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// TODO - Factory
		ISurface surface = new Factory().CreateSurface();
		surface.Create(512, 192*2);
		surface.Updated += Surface_Updated;
		_roomDraw = CPU.Instance.CreateRoomDraw(surface);
		_roomDraw.Draw();

		BaseButton button = GetParent().GetNode<BaseButton>("NextItemButton");
		button.Pressed += NextButton_Pressed;
		button = GetParent().GetNode<BaseButton>("PrevItemButton");
		button.Pressed += PrevButton_Pressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void Surface_Updated(object sender, EventArgs e)
	{
		if(sender is Surface surface)
		{
			Texture = ImageTexture.CreateFromImage(surface.Image);
		}
	}

	private void NextButton_Pressed()
	{
		_roomDraw.NextItem();
	}

	private void PrevButton_Pressed()
	{
		_roomDraw.PreviousItem();
	}

}
