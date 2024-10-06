using Godot;
using System;

public partial class Control : Godot.Control
{
	[Signal]
	public delegate void StartGameEventHandler();
	
	private const string scoreTemplate = "00000";
	private int scoreVal;
	private Label score;
	private Label ready;
	private Label gameOver;
	private Label wellDone;
	private Label title;
	private bool	isTitle;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		score = GetNode<Label>("score");
		ready = GetNode<Label>("ready");
		gameOver = GetNode<Label>("game over");
		wellDone = GetNode<Label>("well done");
		title = GetNode<Label>("title");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(isTitle &&
			Input.IsActionPressed("start"))
		{
			isTitle = false;
			EmitSignal(SignalName.StartGame);	
		}
	}
	
	public void Title()
	{
		isTitle = true;
		title.Show();
		ready.Hide();
		gameOver.Hide();
		wellDone.Hide();
	}
	
	public void NewGame()
	{
		title.Hide();
		ResetScore();
	}

	public void NewLevel()
	{
		wellDone.Hide();
		ready.Show();	
	}
	
	public void StartLevel()
	{
		ready.Hide();
	}
	
	public void GameOver()
	{
		gameOver.Show();	
	}
	
	public void Win()
	{
		wellDone.Show();
	}
	
	private void ResetScore()
	{
		scoreVal = 0;
		UpdateScore();
	}
	
	private void UpdateScore()
	{
		string s = scoreVal.ToString();
		int l = s.Length;
		int n = scoreTemplate.Length - l;
		score.Text = scoreTemplate.Substring(0,n) + s;
	}
	
	public void AddScore(int by)
	{
		scoreVal += by;
		UpdateScore();
	}
}
