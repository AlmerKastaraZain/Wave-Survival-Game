using Godot;
using System;

public partial class DamageableArea : Area2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public int objectHealth = 10;

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void TakeDamage(int damage)
	{
		objectHealth -= damage;
	}
}
