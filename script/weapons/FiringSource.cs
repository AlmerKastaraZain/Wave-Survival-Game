using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

public partial class FiringSource : Node2D
{

	private List<AudioData> audioDatas = new List<AudioData>();

	private void InitializeSounds()
	{
		// Load audio file from resources
		var file = ResourceLoader.Load<AudioStreamWav>("res://sounds/gun/gun1.wav");
		audioDatas.Add(new AudioData(file));

		file = ResourceLoader.Load<AudioStreamWav>("res://sounds/gun/gun2.wav");
		audioDatas.Add(new AudioData(file));

		file = ResourceLoader.Load<AudioStreamWav>("res://sounds/gun/gun3.wav");
		audioDatas.Add(new AudioData(file));
	}

	// Bullet
	private LargeBullet bullet;
	// Called when the node enters the scene tree for the first time.
	private Timer shootingTimer;
	private Timer reloadTimer;
	private bool canShoot = false;
	[Export] private int maxBullet = 10;
	[Export] private float gunCooldown = 1.0f;
	[Export] private float gunSpread = 0.025f;
	[Export] private float reloadCooldown = 1.0f;
	[Export] private AudioStreamPlayer2D audioStreamPlayer2D;
	private bool currentlyReloading = false;
	private bool isActive = true;
	public void ChangeActiveState(bool data)
	{
		isActive = data;

		if (!isActive)
		{
			this.Hide();
			return;
		}

		this.Show();
	}
	private int currentBullet;
	private Node2D body;
	Sprite2D gun;

	public int getMaxBullet()
	{
		return maxBullet;
	}

	public int getCurrentBullet()
	{
		return currentBullet;
	}

	public void HideGun()
	{
		body.Visible = false;
	}

	public void ShowGun()
	{
		body.Visible = true;
	}

	public override void _Ready()
	{
		InitializeSounds();
		shootingTimer = GetNode<Timer>("./ShootingTimer");
		reloadTimer = GetNode<Timer>("./ReloadTimer");
		gun = GetParent<Sprite2D>();
		body = GetParent().GetParent<Node2D>();
		audioStreamPlayer2D = GetChild<AudioStreamPlayer2D>(0);

		currentBullet = maxBullet;

		shootingTimer.Timeout += GunCooldownEnd;
		reloadTimer.Timeout += Reload;
	}


	private void GunCooldownEnd()
	{
		canShoot = true;
		shootingTimer.Stop();
	}

	private void SetGunAnimation()
	{
		double minAngle = Mathf.Sin(Mathf.Tau / 4); // 0.4
		double maxAngle = Mathf.Sin(-Mathf.Tau / 4); // -1

		// Flip Gun when looking down or up
		float lookDirection = gun.Rotation;
		if (minAngle > lookDirection && maxAngle < lookDirection)
		{
			gun.FlipV = true;
			return;
		}

		gun.FlipV = false;
	}

	public void Aiming()
	{
		SetGunAnimation();
	}

	public void Shoot(Godot.Vector2 positionToShoot, bool isPlayerBullet)
	{
		if (!isActive) return;
		SetGunAnimation();

		if (!canShoot || currentBullet <= 0) return;

		Debug.Print(currentBullet.ToString());
		//Summon Bullet
		var bulletInstance = GD.Load<PackedScene>("res://prefab/entity/largeBullet.tscn");
		var instance = bulletInstance.Instantiate();
		LargeBullet bullet = instance as LargeBullet;
		bullet.AngleToVector2(this.GlobalPosition.AngleToPoint(positionToShoot), gunSpread);

		bullet.SetBulletType(isPlayerBullet);


		// Play sound effect
		PlayShootingSound();

		// Create a new bullet and set its position and direction
		GetTree().Root.AddChild(instance);
		bullet.GlobalPosition = this.GlobalPosition;

		currentBullet -= 1;

		if (currentBullet == 0 && !currentlyReloading)
		{
			currentlyReloading = true;
			StartReloadProcess();
			return;
		}

		ResetShootingTimer();
	}

	private void PlayShootingSound()
	{
		int totalSounds = audioDatas.Count();

		RandomNumberGenerator rand = new RandomNumberGenerator();
		int res = rand.RandiRange(0, totalSounds - 1);

		audioStreamPlayer2D.Stream = audioDatas[res].Audio;
		audioStreamPlayer2D.Play();
	}

	private void ResetShootingTimer()
	{
		// Reset the Timer and set CanShoot to false
		canShoot = false;
		shootingTimer.WaitTime = gunCooldown;
		shootingTimer.Start();
	}

	private void StartReloadProcess()
	{
		// Reset the Bullet count
		canShoot = false;
		reloadTimer.WaitTime = reloadCooldown;
		reloadTimer.Start();
	}

	private void Reload()
	{
		// Reset the Bullet count
		currentBullet = maxBullet;
		reloadTimer.Stop();
		ResetShootingTimer();
		currentlyReloading = false;
	}
}
