using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Player : CharacterBody2D
{
	// Character's movement speed

	List<AudioData> hurtAudioDatas = new List<AudioData>();

	[Export] float speed = 200.0f;

	[Export] private float MaxStamina = 100.0f;
	[Export] private float currentStamina = 100.0f;
	public float GetMaxStamina() { return MaxStamina; }
	public float GetCurrentStamina() { return currentStamina; }

	[Export] private float sprintMultiplier = 1.5f;
	[Export] private float runAndGunSpeedPenalty = 0.5f;
	[Export] private float sprintStaminaDrainRate = 30.0f;
	[Export] private float sprintStaminaGainRate = 10.0f;
	[Export] private float sprintCooldown = 1.0f;

	[Export]
	float maxHealth = 100.0f;
	public float GetMaxHealth() { return maxHealth; }

	float currentHealth = 0.0f;
	public float GetCurrentHealth() { return currentHealth; }


	private CameraShake camera;

	public Action PlayerDied;
	enum PlayerState
	{
		Idle,
		Walk,
		Aiming,
		RunAndAim,
		Dead
	}

	private PlayerState state = PlayerState.Idle;
	private bool currentlySprinting = false;
	private bool currentlyShooting = false;
	private bool allowStaminaRegen = false;
	private Timer staminaRegenCooldown;
	private Vector2 velocity;
	private Area2D attackArea;
	private AnimationCharacter playerAnimation;
	private Node2D GunObject;
	private Sprite2D playerSprite;

	private Timer damagedEffectTimer;
	private AudioStreamPlayer2D audioStream;
	private RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
	private PlayerGunController firingSource;

	private void InitializeSounds()
	{
		// Load audio file from resources
		var file = ResourceLoader.Load<AudioStreamWav>("res://sounds/hurt/hurt1.wav");
		hurtAudioDatas.Add(new AudioData(file));

		file = ResourceLoader.Load<AudioStreamWav>("res://sounds/hurt/hurt2.wav");
		hurtAudioDatas.Add(new AudioData(file));

		file = ResourceLoader.Load<AudioStreamWav>("res://sounds/hurt/hurt3.wav");
		hurtAudioDatas.Add(new AudioData(file));
	}
	public void TakeDamage(float damage)
	{
		if (state == PlayerState.Dead)
			return;

		currentHealth -= damage;
		DamagedEffect();
		GD.Print(hurtAudioDatas.Count - 1);
		audioStream.Stream = hurtAudioDatas[randomNumberGenerator.RandiRange(0, hurtAudioDatas.Count - 1)].Audio;
		audioStream.Play();

		if (currentHealth <= 0)
			StartDeathProcess();
	}

	public void AddHealth(int health)
	{
		if (health + currentHealth > maxHealth) currentHealth = maxHealth;

		currentHealth = currentHealth + health;
	}

	public void AddStamina(int stamina)
	{
		if (stamina + currentStamina > MaxStamina) currentStamina = MaxStamina;

		currentStamina = currentStamina + stamina;
	}

	private void StartDeathProcess()
	{
		state = PlayerState.Dead;
		PlayerDied.Invoke();
	}

	private void DamagedEffect()
	{
		damagedEffectTimer.Start();
		damagedEffectTimer.WaitTime = 0.1;

		playerSprite.Modulate = Color.Color8(255, 0, 0);
	}

	private void EndDamagedEffect()
	{
		playerSprite.Modulate = Color.Color8(255, 255, 255);
		damagedEffectTimer.Stop();
	}

	public override void _Ready()
	{
		InitializeSounds();
		damagedEffectTimer = GetNode<Timer>("./DamageTintCooldown");
		playerSprite = GetNode<Sprite2D>("./PlayerSprite");
		// attackArea = GetNode<Area2D>("HitboxArea");
		currentHealth = maxHealth;
		audioStream = GetNode<AudioStreamPlayer2D>("./AudioStreamPlayer2D");

		playerAnimation = GetNode<AnimationCharacter>("AnimationPlayer");
		GunObject = GetNode<Node2D>("GunController");

		staminaRegenCooldown = GetNode<Timer>("./StaminaRegenCooldown");
		firingSource = GetNode<PlayerGunController>("./GunController");

		camera = GetNode<CameraShake>("./PlayerCamera");

		damagedEffectTimer.Timeout += EndDamagedEffect;
		staminaRegenCooldown.Timeout += () =>
		{
			allowStaminaRegen = true;
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (state == PlayerState.Dead)
			return;

		SetState();
		Movement();
		MoveAndSlide();
		SetPlayerAnimation();
		GunVisbility();



		if (allowStaminaRegen && currentStamina <= 100)
			IncreaseStaminaByDeltaTimePassed(delta);
		if (currentlySprinting && this.velocity != Vector2.Zero)
			ReduceStaminaByDeltaTimePassed(delta);
	}
	private void IncreaseStaminaByDeltaTimePassed(double delta)
	{
		currentStamina += sprintStaminaGainRate * (float)delta;
	}

	private void ReduceStaminaByDeltaTimePassed(double delta)
	{
		currentStamina -= sprintStaminaDrainRate * (float)delta;
	}

	private void GunVisbility()
	{
		if (state == PlayerState.Aiming || state == PlayerState.RunAndAim || currentlyShooting)
			GunObject.Visible = true;
		else
			GunObject.Visible = false;
	}


	// Player Attack zomvbies
	public void SetState()
	{
		if (Input.IsKeyPressed(Key.R) || Input.IsMouseButtonPressed(MouseButton.Left))
		{
			Shoot();
		}
		else
			currentlyShooting = false;

		if (Input.IsKeyPressed(Key.F) || Input.IsMouseButtonPressed(MouseButton.Right) && Velocity != Vector2.Zero)
		{
			state = PlayerState.RunAndAim;
			Aim();
			return;

		}
		else if (Input.IsKeyPressed(Key.F) || Input.IsMouseButtonPressed(MouseButton.Right))
		{
			state = PlayerState.Aiming;
			Aim();
			return;
		}

		if (Velocity > Vector2.Zero)
		{
			state = PlayerState.Walk;
			return;
		}

		state = PlayerState.Idle;
	}

	private void Aim()
	{
		GunObject.Rotation = GetGlobalMousePosition().AngleToPoint(this.GlobalPosition);
	}

	private void Shoot()
	{
		if (GunObject.GlobalPosition.DistanceTo(GetGlobalMousePosition()) < 120f)
			return;
		currentlyShooting = true;
		GunObject.Rotation = GetGlobalMousePosition().AngleToPoint(this.GlobalPosition);
		firingSource.Attack(GetGlobalMousePosition());
	}


	public void SetPlayerAnimation()
	{
		if (Velocity != Vector2.Zero && (currentlyShooting || state == PlayerState.RunAndAim))
		{
			playerAnimation.SetWalkingAnimation(this.GlobalPosition.AngleToPoint(GetGlobalMousePosition()));
			return;
		}

		if (state == PlayerState.Aiming || currentlyShooting)
		{
			playerAnimation.SetIdleAnimation(this.GlobalPosition.AngleToPoint(GetGlobalMousePosition()));
			return;
		}


		if (Velocity == Vector2.Zero)
		{
			playerAnimation.SetIdleAnimationOnPreviousDirection();
			return;
		}


		playerAnimation.SetWalkingAnimation(this.Velocity.Angle());
	}

	public void Movement()
	{
		velocity = new Vector2();

		if (Input.IsKeyPressed(Key.Shift) && currentStamina > 0)
		{
			currentlySprinting = true;
			playerAnimation.SpeedUpAnimation(1.5f);
			allowStaminaRegen = false;
			staminaRegenCooldown.WaitTime = sprintCooldown;
			staminaRegenCooldown.Start();
		}
		else
		{
			currentlySprinting = false;
			playerAnimation.SetAnimationSpeedToDefault();
		}

		if (Input.IsKeyPressed(Key.W))
			velocity += Vector2.Up;

		if (Input.IsKeyPressed(Key.A))
			velocity += Vector2.Left;

		if (Input.IsKeyPressed(Key.S))
			velocity += Vector2.Down;

		if (Input.IsKeyPressed(Key.D))
			velocity += Vector2.Right;

		velocity = velocity.Normalized() * speed;

		if (currentlySprinting == true)
			velocity = sprintMultiplier * velocity;

		if (currentlyShooting || state == PlayerState.Aiming || state == PlayerState.RunAndAim)
			if (firingSource.GetCurrentGunType() == GunType.Rifle) velocity = velocity * runAndGunSpeedPenalty;


		Velocity = velocity;
	}
}
