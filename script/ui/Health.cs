using Godot;
using System;

public partial class Health : Label
{
	// Called when the node enters the scene tree for the first time.
	Player player;
	public override void _Ready()
	{
		player = GetNode<Player>("%Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Update the text to display the current health of the character.
		Text = "Health: " + player.GetNode<DamageableArea>("%Player/PlayerHitbox").objectHealth.ToString();
	}
}
