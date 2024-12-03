using Godot;
using System;

public partial class Healthbar : TextureProgressBar
{
	// Called when the node enters the scene tree for the first time.
	private Player player;
	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("./Root/Player");
		this.MaxValue = player.GetMaxHealth();
		this.MinValue = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Value = player.GetCurrentHealth();
	}
}
