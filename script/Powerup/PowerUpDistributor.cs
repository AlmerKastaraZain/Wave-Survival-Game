using Godot;
using System;

public partial class PowerUpDistributor : Node2D
{

	static string defaultEnemyRes = "res://prefab/powerup/";


	class PowerUpItem
	{
		public static string healthPowerUp = defaultEnemyRes + "health.tscn";
		public static string energyPowerUp = defaultEnemyRes + "energy.tscn";
	}

	private int MaxPowerUpOnMap = 3;
	private int currentPowerUpOnMap;
	private Node2D PowerUpObject;
	private Timer timer;

	private TileMapLayer tilemap;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tilemap = GetNode<TileMapLayer>("./TileMapLayer");
		PowerUpObject = GetNode<Node2D>("./PowerUpObject");
		timer = GetNode<Timer>("./Timer");

		timer.Timeout += AddPowerUp;
		timer.Timeout += RestartTimer;
	}

	private void AddPowerUp()
	{
		if (PowerUpObject.GetChildren().Count > MaxPowerUpOnMap) return;

		Godot.Collections.Array<Vector2I> tiles = tilemap.GetUsedCellsById();

		RandomNumberGenerator rng = new RandomNumberGenerator();
		Vector2I tilePos = tiles[rng.RandiRange(0, tiles.Count)];

		int xPos = tilePos.X;
		int yPos = tilePos.Y;

		int rand = rng.RandiRange(1, 2);
		if (rand == 1)
		{
			var powerUpInstance = GD.Load<PackedScene>(PowerUpItem.healthPowerUp);
			var Instance = powerUpInstance.Instantiate();
			var instance = Instance as HealthPowerUp;
			Vector2I position = new Vector2I();
			position.X = xPos;
			position.Y = yPos;
			Vector2 pos = tilemap.MapToLocal(position);
			instance.GlobalPosition = pos;
			PowerUpObject.AddChild(Instance);
		}
		else if (rand == 2)
		{
			var powerUpInstance = GD.Load<PackedScene>(PowerUpItem.energyPowerUp);
			var Instance = powerUpInstance.Instantiate();
			var instance = Instance as EnergyPowerUp;
			Vector2I position = new Vector2I();
			position.X = xPos;
			position.Y = yPos;
			Vector2 pos = tilemap.MapToLocal(position);
			instance.GlobalPosition = pos;
			PowerUpObject.AddChild(Instance);
		}
	}

	private void RestartTimer()
	{
		timer.Start();
	}
}
