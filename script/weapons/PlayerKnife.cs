using Godot;
using System;

public partial class PlayerKnife : Node2D
{
	private Area2D attackArea;
	[Export] float attackDamage = 10.0f;
	[Export] float attackCooldown = 1.0f;
	[Export] private SoldierCharger charger;

	private bool canAttack = true;
	private Timer attackCooldownTimer;
	private Player player;
	private KnifeAnimationPlayer knifeAnimationPlayer;

	private Node2D knifeTransform;

	private bool isActive = true;
	private bool canSwing = true;

	public void ChangeActiveState(bool data)
	{
		isActive = data;

		if (!isActive)
		{
			knifeAnimationPlayer.Stop();
			this.Hide();
			return;
		}

		knifeAnimationPlayer.Stop();
		this.Show();
	}
	public Action swingKnife;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("./Root/Player");
		attackArea = GetNode<Area2D>("./KnifeTransform/Attack");

		knifeTransform = GetNode<Node2D>("./KnifeTransform");

		attackCooldownTimer = GetNode<Timer>("./AttackCooldownTimer");
		knifeAnimationPlayer = GetNode<KnifeAnimationPlayer>("./KnifeTransform/KnifeAnimationPlayer");

		attackCooldownTimer.Timeout += AllowSwing;
	}

	private void AllowSwing()
	{
		canSwing = true;
	}


	public void Swing()
	{
		if (!isActive || !canSwing)
			return;

		knifeAnimationPlayer.PlaySwingAnimation();

		Godot.Collections.Array<Area2D> OverlappedArea = attackArea.GetOverlappingAreas();
		foreach (var area in OverlappedArea)
		{
			if (area.HasMeta("Enemy"))
			{
				var enemy = area.GetParent<SoldierCharger>();
				enemy.TakeDamage(attackDamage);
			}
		}

		attackCooldownTimer.WaitTime = attackCooldown;
		attackCooldownTimer.Start();

		canSwing = false;
	}
}
