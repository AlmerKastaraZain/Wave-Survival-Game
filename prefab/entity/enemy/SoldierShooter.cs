using Godot;
using System;

public partial class SoldierShooter : Node2D
{
	private Player player;
	private GunEnemyScript gunEnemy;
	private Knife knife;

	public override void _Ready()
	{
		player = GetTree().Root.GetNode<Player>("./Root/Player");
		gunEnemy = GetNode<GunEnemyScript>("./Gun");
		knife = GetNode<Knife>("./Knife");
	}

	public override void _Process(double delta)
	{
		if (player.GlobalPosition.DistanceTo(this.GlobalPosition) < 200f)
		{
			gunEnemy.ChangeActiveState(false);
			gunEnemy.Hide();
			knife.ChangeActiveState(true);
			knife.Show();
			return;
		}
		gunEnemy.ChangeActiveState(true);
		gunEnemy.Show();
		knife.ChangeActiveState(false);
		knife.Hide();
	}
}
