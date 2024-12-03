using Godot;
using System;

public partial class BulletCollison : CollisionShape2D
{

	private void onCollisionEnter2D(CollisionShape2D other)
	{
		if (other.GetOwner<SoldierCharger>() is SoldierCharger)
		{
			// 
			CollisionShape2D a = new CollisionShape2D();
			GD.Print("ENEMY");
		}

		// Summon particle here
		GD.Print("Hail");

		Dispose();
	}


}
