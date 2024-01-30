using Godot;
using System;

public partial class main : Node
{
	[Export]
	public PackedScene BlockScene { get; set; }

	[Export]
	public PackedScene BombScene { get; set; }
	
	private bool bombActive;
	private Bomb bomb;
	private int bombCount;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bomb = GetNode<Bomb>("Bomb");
		RemoveChild(bomb);
		NewGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void NewGame()
	{
		bomb.Hide();
		bombActive = false;
		var player = GetNode<Plane>("Plane");
		var pos = GetNode<Marker2D>("startpoint");
		player.Start(pos.Position);
		//Build();
	}
	
	private void Build()
	{
		Marker2D marker = GetNode<Marker2D>("buildings");
		
		Vector2 nextPos = marker.Position;
		
		for(int n=0; n<24;n++)
		{
			uint height = (GD.Randi() % 10)+5;
			float w = 0;
			// Reset Y pos
			nextPos.Y = marker.Position.Y;
			
			for(int h=0; h<height; h++)
			{
				var block = BlockScene.Instantiate<RigidBody2D>();
				CollisionShape2D item = block.GetNode<CollisionShape2D>("CollisionShape2D");
				Vector2 blockRect = item.Shape.GetRect().Size;
				w = blockRect.X;
				block.Position = nextPos;
				AddChild(block);
				nextPos.Y -= blockRect.Y;
			}
				
			nextPos.X += w;
		}
	}

	private void OnPlaneFire(Vector2 position)
	{
		if(!bombActive)
		{
			bomb.Position = position;
			ActivateBomb();
		}
	}	
	
	private void _on_plane_plane_hit(Node body)
	{
		if(body.Name == "Goal")
		{
			GD.Print("Hooray!");
		}
		else
		{
			GD.Print("Crash");
		}
	}

	private void _on_bomb_hit(Node2D body)
	{
		if(body.Name == "grass")
		{
			DisableBomb();
		}
		else
		{
			body.Hide();
			body.CallDeferred("free");
			bombCount--;
			if(bombCount <= 0)
			{
				DisableBomb();
			}
		}
	}
	
	private void ActivateBomb()
	{
		bombCount = 3;
		bombActive = true;
		AddChild(bomb);
		bomb.Show();				
	}
	
	private void DisableBomb()
	{
		bomb.Hide();
		bombActive = false;
		CallDeferred("remove_child", bomb);		
	}
}
