using Godot;
using System;
using System.Diagnostics;

public partial class LargeBullet : CharacterBody2D
{
	[Export] private float speed = 2000.0f;
	[Export] private float spread = 0.025f;

	private float damage = 2.5f;
	private bool hasAttacked = false;
	private Area2D area2D;
	private Vector2 bulletDirection = new Vector2();
	private RandomNumberGenerator rand = new RandomNumberGenerator();
	private bool isPlayerBullet = false;
	public void SetBulletType(bool bulletType)
	{
		isPlayerBullet = bulletType;
	}

	public override void _Ready()
	{
		// attackArea = GetNode<Area2D>("HitboxArea");
		area2D = GetNode<Area2D>("Area2D");

		area2D.AreaEntered += OnAreaEntered;
	}


	public void SetDamage(float gunDamage)
	{
		damage = gunDamage;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (hasAttacked)
			return;

		if (area.HasMeta("Enemy") == true && isPlayerBullet)
		{
			var parent = area.GetParent<SoldierCharger>();
			parent.TakeDamage(damage);

			hasAttacked = true;
			speed = 0;
			this.QueueFree();
		}

		if (area.HasMeta("Player") && !isPlayerBullet)
		{
			var parent = area.GetParent<Player>();
			parent.TakeDamage(damage);

			hasAttacked = true;
			speed = 0;
			this.QueueFree();
		}

		if (area.HasMeta("Wall") == true)
		{
			speed = 0;
			hasAttacked = true;

			this.QueueFree();

		}

	}



	public void AngleToVector2(float angle, float bulletDeviation)
	{
		spread = bulletDeviation;
		float deviation_angle = (float)Math.PI * spread;
		angle += rand.RandfRange(-deviation_angle, deviation_angle);

		bulletDirection = Vector2.FromAngle(angle);
		this.Rotate(angle);
	}


	public override void _Process(double delta)
	{
		Vector2 velocity = new Vector2();

		velocity += bulletDirection.Normalized() * speed;
		Velocity = velocity;
		MoveAndSlide();
	}
}
