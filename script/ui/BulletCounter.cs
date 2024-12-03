using Godot;
using System;

public partial class BulletCounter : RichTextLabel
{
	private FiringSource playerFiringSource;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerFiringSource = GetNode<FiringSource>("%PlayerFiringSource");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Text = $"{playerFiringSource.getCurrentBullet()}/{playerFiringSource.getMaxBullet()}";
	}
}
