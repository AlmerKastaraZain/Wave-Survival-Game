using Godot;
using System;

public partial class GameInterface : Control
{
	// Called when the node enters the scene tree for the first time.
	CharacterBody2D player;
	public override void _Ready()
	{
		Position = GetScreenPosition();
		player = GetNode<CharacterBody2D>("%Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = GetScreenPosition();
	}
}
