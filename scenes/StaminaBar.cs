using Godot;
using System;

public partial class StaminaBar : TextureProgressBar
{
	private Player player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("./Root/Player");
		this.MaxValue = player.GetMaxStamina();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Value = player.GetCurrentStamina();
	}
}
