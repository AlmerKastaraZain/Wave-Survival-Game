using Godot;
using System;

public partial class PlayAgainButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Pressed += PlayGame;

	}

	private void PlayGame()
	{
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}
}
