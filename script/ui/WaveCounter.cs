using Godot;
using System;

public partial class WaveCounter : RichTextLabel
{
	private WaveController waveController;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		waveController = GetTree().Root.GetNode<WaveController>("Root/WaveController");
		waveController.RefreshWaveUI += UpdateWaveCounter;
	}
	private void UpdateWaveCounter(int wave)
	{
		this.Text = $"Wave: {wave}";
	}
}
