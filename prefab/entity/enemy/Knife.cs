using Godot;
using System;

public partial class Knife : Node2D
{
	private Area2D attackArea;
	[Export] float attackDamage = 60.0f;
	[Export] float attackChargeDuration = 0.6f;
	[Export] float attackCooldown = 1.0f;
	[Export] private SoldierCharger charger;

	private bool canAttack = true;
	private Timer chargeUpTimer;
	private Timer attackCooldownTimer;
	private Player player;
	private KnifeAnimationPlayer knifeAnimationPlayer;

	private Node2D knifeTransform;

	private bool isActive = true;

	public void ChangeActiveState(bool val)
	{
		isActive = val;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("./Root/Player");
		attackArea = GetNode<Area2D>("./KnifeTransform/Attack");

		knifeTransform = GetNode<Node2D>("./KnifeTransform");

		chargeUpTimer = GetNode<Timer>("./ChargeUpTimer");
		attackCooldownTimer = GetNode<Timer>("./AttackCooldownTimer");
		knifeAnimationPlayer = GetNode<KnifeAnimationPlayer>("./KnifeTransform/KnifeAnimationPlayer");

		chargeUpTimer.Timeout += AttackPlayer;
		attackCooldownTimer.Timeout += EndAttackCooldown;
	}

	private void SetKnifePosition()
	{
		double Angle = this.GlobalPosition.AngleToPoint(player.GlobalPosition);
		knifeTransform.Rotation = (float)Angle;
	}


	public override void _Process(double delta)
	{
		if (!isActive)
			return;

		SetKnifePosition();

		if (!attackArea.HasOverlappingAreas())
			return;

		var allAreas = attackArea.GetOverlappingAreas();
		foreach (var area in allAreas)
		{
			if (area.HasMeta("Player") && chargeUpTimer.IsStopped() && canAttack)
			{
				chargeUpTimer.WaitTime = attackChargeDuration;
				chargeUpTimer.Start();
			}
		}
	}



	private void AttackPlayer()
	{
		knifeAnimationPlayer.PlaySwingAnimation();

		var allAreas = attackArea.GetOverlappingAreas();
		foreach (var area in allAreas)
		{
			if (area.HasMeta("Player"))
			{
				var player = area.GetParent<Player>();
				player.TakeDamage(attackDamage);
				chargeUpTimer.Stop();

				canAttack = false;
				attackCooldownTimer.WaitTime = attackCooldown;
				attackCooldownTimer.Start();
			}
		}

		chargeUpTimer.Stop();

		canAttack = false;
		attackCooldownTimer.WaitTime = attackCooldown;
		attackCooldownTimer.Start();
	}

	private void EndAttackCooldown()
	{
		canAttack = true;
		attackCooldownTimer.Stop();
	}
}
