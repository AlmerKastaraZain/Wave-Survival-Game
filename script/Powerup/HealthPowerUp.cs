using Godot;
using System;

public partial class HealthPowerUp : Node2D
{
	private Player player;
	private Area2D area;
	[Export] private int healthRecieved;
	private AudioStreamPlayer2D audioStream;
	private bool alreadyCollected = false;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("./Root/Player");
		area = GetNode<Area2D>("./Area2D");
		audioStream = GetNode<AudioStreamPlayer2D>("./AudioStreamPlayer2D");
		area.AreaEntered += AreaEntered;

		audioStream.Finished += AudioEnded;
	}

	private void AreaEntered(Area2D area)
	{
		if (alreadyCollected) return;

		if (area.HasMeta("Player"))
		{

			player.AddHealth(healthRecieved);
			audioStream.Play();
			this.Visible = false;

			alreadyCollected = true;
		}
	}

	private void AudioEnded()
	{
		this.QueueFree();
	}
}
