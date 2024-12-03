using Godot;
using System;

public partial class PlayButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Pressed += PlayGame;

	}

	private void PlayGame()
	{
		GetTree().ChangeSceneToFile("res://scenes/Main.tscn");
	}
}
