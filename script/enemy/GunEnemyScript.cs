using Godot;
using System;

public partial class GunEnemyScript : Node2D
{
	private Player player;
	private FiringSource enemyFiringSource;
	private RayCast2D raycast2D;
	private Timer timer;
	private RandomNumberGenerator randomNumberGenerator;

	private bool isActive = true;

	public void ChangeActiveState(bool val)
	{
		isActive = val;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("Root/Player");
		enemyFiringSource = GetNode<FiringSource>("./Sprite2D/EnemyFiringSource");
		raycast2D = GetNode<RayCast2D>("./RayCast2D");
		timer = GetNode<Timer>("./Timer");
		randomNumberGenerator = new RandomNumberGenerator();
		timer.WaitTime = randomNumberGenerator.RandfRange(0.25f, 0.32f);

		timer.Start();

		timer.Timeout += Shoot;
	}

	private void Shoot()
	{
		if (!isActive)
			return;

		this.Rotation = player.GlobalPosition.AngleToPoint(this.GlobalPosition);
		enemyFiringSource.Aiming();

		raycast2D.TargetPosition = player.GlobalPosition;
		GodotObject area = raycast2D.GetCollider();
		if (area != null)
			if (area.HasMeta("Wall"))
				return;

		enemyFiringSource.Shoot(player.GlobalPosition, false);
	}
}
