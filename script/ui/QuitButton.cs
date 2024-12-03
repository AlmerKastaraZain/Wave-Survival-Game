using Godot;
using System;

public partial class QuitButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		this.Pressed += ExitGame;
	}

	private void ExitGame()
	{
		GetTree().Quit();
	}
}
