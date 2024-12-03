using Godot;
using System;

public partial class GoToMenuButton : Button
{
	public override void _Ready()
	{
		this.Pressed += PlayGame;

	}

	private void PlayGame()
	{
		GetTree().ChangeSceneToFile("res://scenes/Main.tscn");
	}
}
