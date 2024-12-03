using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class SoldierCharger : CharacterBody2D
{
	private List<AudioData> deathAudioDatas = new List<AudioData>();
	private List<AudioData> hurtAudioDatas = new List<AudioData>();


	// Zombie Properties
	float speed;
	float currentHealth;
	public float getCurrentHealth() { return currentHealth; }

	[Export]
	float maxHealth = 10f;
	public float getMaxHealth() { return maxHealth; }
	[Export]
	float baseSpeed = 150f;
	[Export]
	float chargeSpeed = 100.0f;
	[Export]
	float attackSpeed = 10f;
	[Export]
	float attackRadius = 10f;
	[Export]
	int minDamage = 1;
	[Export]
	int maxDamage = 10;
	[Export]
	bool canAttack = false;
	bool canMovement = true;

	public Player player;
	NavigationAgent2D navigator;
	Sprite2D sprite;
	Timer timer;
	Timer damagedEffectTimer;
	Timer attackTimer;
	Area2D attackArea;
	AnimationPlayer slashAnimation;
	private AnimationCharacter enemyAnimation;
	private AudioStreamPlayer2D audioStream;
	private RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();



	private void InitializeSounds()
	{
		// Load audio file from resources
		var file = ResourceLoader.Load<AudioStreamWav>("res://sounds/hurt/hurt1.wav");
		hurtAudioDatas.Add(new AudioData(file));

		file = ResourceLoader.Load<AudioStreamWav>("res://sounds/hurt/hurt2.wav");
		hurtAudioDatas.Add(new AudioData(file));

		file = ResourceLoader.Load<AudioStreamWav>("res://sounds/hurt/hurt3.wav");
		hurtAudioDatas.Add(new AudioData(file));

		file = ResourceLoader.Load<AudioStreamWav>("res://sounds/death/death.wav");
		deathAudioDatas.Add(new AudioData(file));
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitializeSounds();

		speed = baseSpeed;
		currentHealth = maxHealth;
		enemyAnimation = GetNode<AnimationCharacter>("./AnimationPlayer");
		timer = GetNode<Timer>("./Timer");
		damagedEffectTimer = GetNode<Timer>("./DamageTintCooldown");
		navigator = GetNode<NavigationAgent2D>("./NavigationAgent2D");
		player = GetTree().Root.GetNode<Player>("./Root/Player");
		sprite = GetNode<Sprite2D>("./Sprite2D");
		audioStream = GetNode<AudioStreamPlayer2D>("./AudioStreamPlayer2D");

		timer.Timeout += _on_timer_timeout;

		navigator.VelocityComputed += Navigator_VelocityComputed;

		damagedEffectTimer.Timeout += EndDamagedEffect;
	}



	public void TakeDamage(float damage)
	{
		currentHealth -= damage;
		DamagedEffect();

		audioStream.Stream = hurtAudioDatas[randomNumberGenerator.RandiRange(0, hurtAudioDatas.Count - 1)].Audio;

		if (currentHealth <= 0)
			StartDeathProcess();
	}

	private void DamagedEffect()
	{
		damagedEffectTimer.Start();
		damagedEffectTimer.WaitTime = 0.1;

		sprite.Modulate = Color.Color8(255, 0, 0);
	}

	private void EndDamagedEffect()
	{
		sprite.Modulate = Color.Color8(255, 255, 255);
		damagedEffectTimer.Stop();
	}

	private async void StartDeathProcess()
	{
		audioStream.Stream = deathAudioDatas[randomNumberGenerator.RandiRange(0, deathAudioDatas.Count - 1)].Audio;
		audioStream.Play();


		await ToSignal(GetTree().CreateTimer(0.2), "timeout");

		this.QueueFree();
	}

	private void QueueDeath()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		enemyAnimation.SetWalkingAnimation(this.Velocity.Angle());

		if (navigator.IsNavigationFinished()) return;
		Vector2 dir = ToLocal(navigator.GetNextPathPosition()).Normalized();
		Vector2 intendedVelocity = dir * speed;
		navigator.SetVelocity(intendedVelocity);
	}

	private void Navigator_VelocityComputed(Vector2 safeVelocity)
	{
		Velocity = safeVelocity;
		if (!GetTree().Paused) MoveAndSlide();
	}

	// Path finding
	private void MakePath()
	{
		// Area2D area = getShortestArea2D(HearingDistance.GetOverlappingAreas());
		navigator.TargetPosition = player.GlobalPosition;
	}

	private void _on_timer_timeout()
	{
		MakePath();
	}

}
