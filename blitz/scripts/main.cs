using Godot;
using System;

public partial class main : Node
{
	[Export]
	public PackedScene BombScene { get; set; }
	
	private bool bombActive;
	private Bomb bomb;
	private int bombCount;
	private Control control;
	private Plane player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		control = GetNode<Control>("Control");
		bomb = GetNode<Bomb>("Bomb");
		player = GetNode<Plane>("Plane");
		Title();
	}

	private void Title()
	{
		player.Title();
		control.Title();		
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_control_start_game()
	{
		NewGame();
	}
	
	private void NewGame()
	{
		RemoveChild(bomb);
		control.NewGame();
		player.NewGame();
		NewLevel();
	}
	
	private void NewLevel()
	{
		control.NewLevel();
		bomb.Hide();
		bombActive = false;
		Builder builder = GetNode<Builder>("builder");
		builder.Build();
		Timer timer = GetNode<Timer>("StartTimer");
		timer.Start();		
	}
	
	private void StartLevel()
	{
		control.StartLevel();
		var pos = GetNode<Marker2D>("startpoint");
		player.StartLevel(pos.Position);
	}
	
	private void _on_start_timer_timeout()
	{
		StartLevel();
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
		control.GameOver();
		Timer timer = GetNode<Timer>("GameOverTimer");
		timer.Start();
	}

	private void _on_plane_plane_landed()
	{
		control.Win();
		Timer timer = GetNode<Timer>("WinTimer");
		timer.Start();
	}

	private void _on_win_timer_timeout()
	{
		player.Win();
		Timer timer = GetNode<Timer>("NewLevelTimer");
		timer.Start();
	}

	private void _on_new_level_timer_timeout()
	{
		NewLevel();
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
			control.AddScore(10);
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

	private void _on_game_over_timer_timeout()
	{
		Builder builder = GetNode<Builder>("builder");
		builder.Clear();
		Title();
	}
}
