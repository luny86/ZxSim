using Godot;
using System;

public partial class Bomb : Area2D
{
	[Signal]
	public delegate void HitEventHandler(Node2D body);
	
	private readonly int speed = 300;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = new Vector2(0,1);
		velocity = velocity.Normalized() * speed;
	
		Position += velocity * (float)delta;		
	
	}
	
	private void _on_body_entered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit, body);
	}
}
