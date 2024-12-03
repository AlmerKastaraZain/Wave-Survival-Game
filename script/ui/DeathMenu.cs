using Godot;
using System;

public partial class DeathMenu : Control
{
	private Player player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("./Root/Player");

		player.PlayerDied += OnPlayerDied;
	}

	private void OnPlayerDied()
	{
		ShowMenu();
	}

	public void ShowMenu()
	{
		this.Show();

		GetTree().Paused = true;
	}
}
