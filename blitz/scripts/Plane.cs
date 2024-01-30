using Godot;
using System;

public partial class Plane : Area2D
{
	private enum Mode
	{
		Fly,
		WaitingFall,
		Fall,
		Stop
	};
	
	[Signal]
	public delegate void FireEventHandler(Vector2 position);

	[Signal]
	public delegate void PlaneHitEventHandler(Node body);
	
	private readonly int speed = 200;
	private Vector2 screenSize;
	private Mode	hit;
	private CollisionShape2D endHit;
	private CollisionShape2D bottomHit;
	
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.screenSize = GetViewportRect().Size;
		endHit = GetNode<CollisionShape2D>("EndHit");
		bottomHit = GetNode<CollisionShape2D>("BottomHit");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(hit == Mode.Fly &&
			Input.IsActionPressed("fire"))
		{
			Vector2 from = Position;
			from.X += 8;
			from.Y += 16;
			EmitSignal(SignalName.Fire, from);			
		}
		
		switch(hit)
		{
			case Mode.Fly:
				FlyHoriz(delta);
				break;
				
			case Mode.Fall:
				Fall(delta);
				break;
		}
	}
	
	private void FlyHoriz(double delta)
	{
		Move(delta, new Vector2(1,0));
	}

	private void Fall(double delta)
	{
		if(bottomHit.Disabled == true)
		{
			bottomHit.Disabled = false;
		}
		Move(delta, new Vector2(0,1));	
	}	
	
	private void Move(double delta, Vector2 velocity)
	{	
		velocity = velocity.Normalized() * speed;
	
		Position += velocity * (float)delta;		
		if(Position.X > this.screenSize.X)
		{
			Position = new Vector2(-100, Position.Y+ 24);
		}		
	}
	
	public void Start(Vector2 position)
	{
		hit = Mode.Fly;
		endHit.Disabled = false;
		bottomHit.Disabled = true;
		Position = position;
		var anim = GetNode<AnimatedSprite2D>("image");
		anim.Play();
		Show();	
	}

	private void _on_body_entered(Node body)
	{
		switch(hit)
		{
			case Mode.Fly:
				SetAsCrashing();
				break;
				
			case Mode.Fall:
				hit = Mode.Stop;
				EmitSignal(SignalName.PlaneHit, body);
				break;
		}
	}
	
	private void SetAsCrashing()
	{
		hit = Mode.WaitingFall;
		CallDeferred(nameof(UpdateCollision));
		var anim = GetNode<AnimatedSprite2D>("image");
		anim.Stop();		
	}
	
	private void UpdateCollision()
	{
		endHit.Disabled = true;
		bottomHit.Disabled = false;
		hit = Mode.Fall;
	}
}
