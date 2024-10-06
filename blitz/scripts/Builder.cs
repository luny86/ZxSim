using Godot;
using System;

public partial class Builder : Area2D
{
	[Export]
	public PackedScene BlockScene { get; set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void Build()
	{
		Marker2D marker = GetNode<Marker2D>("buildings");
		
		Vector2 nextPos = marker.Position;
		int index = 1;
		
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
				block.Name = "block"+index.ToString();
				AddChild(block);
				nextPos.Y -= blockRect.Y;
			}
				
			nextPos.X += w;
		}
	}
	
	public void Clear()
	{
		foreach(Node node in this.GetChildren())
		{
			if(node.Name.ToString().StartsWith("@RigidBody", StringComparison.OrdinalIgnoreCase) ||
			node.Name.ToString().StartsWith("block", StringComparison.OrdinalIgnoreCase))
			{
				node.QueueFree();
			}
		}
	}
}
