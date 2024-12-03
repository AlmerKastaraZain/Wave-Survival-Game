using Godot;
using System;

public partial class BossController : Node2D
{
	BossHealth bossHealth;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bossHealth = GetTree().Root.GetNode<BossHealth>("./Root/Player/PlayerCamera/GameUI/BossHealth/Panel/BossHealthCounter");
		bossHealth.SetEnemy(GetParent<SoldierCharger>());
	}


}
