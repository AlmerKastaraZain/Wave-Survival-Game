using Godot;
using System;

public partial class WinMenu : Control
{
	private WaveController waveController;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		waveController = GetTree().Root.GetNode<WaveController>("./Root/WaveController");

		waveController.WaveHasEnded += OnWaveEnded;
	}

	private void OnWaveEnded()
	{
		ShowMenu();
	}

	public void ShowMenu()
	{
		this.Show();

		GetTree().Paused = true;
	}
}
