using Godot;
using System;

public partial class BossHealth : TextureProgressBar
{
	private SoldierCharger Enemy;

	public void SetEnemy(SoldierCharger enemy) { Enemy = enemy; }
	// Called when the node enters the scene tree for the first time.
	public override void _Process(double delta)
	{
		if (Enemy == null)
		{
			DisableBossHealthBar();
			return;
		}

		EnableBossHealthBar();
		this.MaxValue = Enemy.getMaxHealth();
		this.Value = Enemy.getCurrentHealth();

		if (Enemy.getCurrentHealth() <= 0) Enemy = null;

	}

	private void DisableBossHealthBar()
	{
		GetParent<Panel>().GetParent<Godot.Control>().Hide();
	}

	private void EnableBossHealthBar()
	{
		GetParent<Panel>().GetParent<Godot.Control>().Show();
	}
}
