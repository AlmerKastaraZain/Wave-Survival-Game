using Godot;
using System;

public enum GunType
{
	Rifle,
	Pistol,
	Knife
}
public partial class PlayerGunController : Node2D
{
	private Player player;

	private GunType currentGunType = GunType.Rifle;
	public GunType GetCurrentGunType() { return currentGunType; }
	private FiringSource Rifle;
	private FiringSource Pistol;
	private PlayerKnife Knife;


	public override void _Ready()
	{
		currentGunType = GunType.Rifle;


		Rifle = GetNode<FiringSource>("./GunRifle/Gun/PlayerFiringSource");
		Pistol = GetNode<FiringSource>("./GunPistol/Gun/PlayerFiringSource");
		Knife = GetNode<PlayerKnife>("./Knife");
		player = GetParent<Player>();
		GD.Print(Rifle);

		Knife.Visible = false;
		Rifle.ShowGun();
		Pistol.HideGun();

		Knife.ChangeActiveState(false);
		Rifle.ChangeActiveState(true);
		Pistol.ChangeActiveState(false);
	}

	public void Attack(Vector2 direction)
	{
		switch (currentGunType)
		{
			case GunType.Rifle:
				Rifle.Shoot(direction, true);
				break;
			case GunType.Pistol:
				Pistol.Shoot(direction, true);
				break;
			case GunType.Knife:
				Knife.Swing();
				break;
			default:
				GD.Print("Unknown Gun Type");
				break;
		}
	}


	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Key1))
		{
			currentGunType = GunType.Rifle;

			Knife.ChangeActiveState(false);
			Rifle.ChangeActiveState(true);
			Pistol.ChangeActiveState(false);

			Knife.Visible = false;
			Rifle.ShowGun();
			Pistol.HideGun();
		}
		else if (Input.IsKeyPressed(Key.Key2))
		{
			currentGunType = GunType.Pistol;

			Knife.ChangeActiveState(false);
			Rifle.ChangeActiveState(false);
			Pistol.ChangeActiveState(true);

			Knife.Visible = false;
			Rifle.HideGun();
			Pistol.ShowGun();
		}
		else if (Input.IsKeyPressed(Key.Key3))
		{
			currentGunType = GunType.Knife;

			Knife.ChangeActiveState(true);
			Rifle.ChangeActiveState(false);
			Pistol.ChangeActiveState(false);

			Knife.Visible = true;
			Rifle.HideGun();
			Pistol.HideGun();
		}
	}
}
