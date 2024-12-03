using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public partial class WaveController : Node2D
{
	static string defaultEnemyRes = "res://prefab/entity/enemy/";


	public Action<int> RefreshWaveUI;
	public Action WaveHasEnded;
	class Spawner_Unit
	{
		public static string chargerRes = defaultEnemyRes + "soldierCharger.tscn";
		public static string shooterRes = defaultEnemyRes + "soldierShooter.tscn";
		public static string PKIBoss = defaultEnemyRes + "PKIBoss.tscn";
	}

	class Wave_Unit
	{
		public string Unit;
		public int Count;
		public Wave_Unit(string unit, int count)
		{
			Unit = unit;
			Count = count;
		}
	}

	class Waves
	{
		public Wave_Unit[] units;
		public Waves(Wave_Unit[] units)
		{
			this.units = units;
		}
	}

	private Waves[] gameWaves = new Waves[10]
	{
		new Waves(new Wave_Unit[]
			{
				new Wave_Unit(Spawner_Unit.chargerRes, 5),
			}
		),
		new Waves(new Wave_Unit[]
			{
				new Wave_Unit(Spawner_Unit.chargerRes, 10),
			}
		),
		new Waves(new Wave_Unit[]
			{
				new Wave_Unit(Spawner_Unit.chargerRes, 15),
			}
		),
		new Waves(new Wave_Unit[]
			{
				new Wave_Unit(Spawner_Unit.chargerRes, 20),
			}
		),
		new Waves(new Wave_Unit[]
			{
				new Wave_Unit(Spawner_Unit.chargerRes, 10),
				new Wave_Unit(Spawner_Unit.shooterRes, 1)
			}
		),
		new Waves(new Wave_Unit[]
			{
				new Wave_Unit(Spawner_Unit.chargerRes, 15),
				new Wave_Unit(Spawner_Unit.shooterRes, 5)
			}
		),
		new Waves(new Wave_Unit[]
			{
				new Wave_Unit(Spawner_Unit.chargerRes, 20),
				new Wave_Unit(Spawner_Unit.shooterRes, 5)
			}
		),
		new Waves(new Wave_Unit[]
			{
				new Wave_Unit(Spawner_Unit.chargerRes, 10),
				new Wave_Unit(Spawner_Unit.shooterRes, 10)
			}
		),
		new Waves(new Wave_Unit[]
			{
				new Wave_Unit(Spawner_Unit.chargerRes, 5),
				new Wave_Unit(Spawner_Unit.shooterRes, 20)
			}
		),
		new Waves(new Wave_Unit[]
			{
				new Wave_Unit(Spawner_Unit.shooterRes, 15),
				new Wave_Unit(Spawner_Unit.PKIBoss, 1)
			}
		),
	};

	private List<PackedScene> spawningList = new List<PackedScene>();
	// TODO: Make wave script

	public int currentWave = 1;
	private int maxWave = 10;
	private int enemyCount = 0;
	private bool hasWaveEnded = true;
	private Godot.Collections.Array<Godot.Node> WaveSpawner;
	private Timer spawnerTimer;
	private Node2D enemyNode;
	private bool dontSpawnWave = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		WaveSpawner = GetChildren();

		enemyNode = GetNode<Node2D>("%Enemy");
		RefreshWaveUI.Invoke(currentWave);
		spawnerTimer = GetNode<Timer>("SpawnerTimer");
		maxWave = gameWaves.Length;
		spawnerTimer.Timeout += SpawnListedEnemy;
		StartWave();
	}

	private void EndWave()
	{
		currentWave += 1;
		if (currentWave > maxWave)
		{
			WaveHasEnded.Invoke();
			dontSpawnWave = true;
		}




		RefreshWaveUI.Invoke(currentWave);
		StartWave();

	}


	private void StartWave()
	{
		if (dontSpawnWave) return;

		foreach (var item in gameWaves[currentWave - 1].units)
		{
			for (int i = 0; i < item.Count; i++)
			{
				var enemyInstance = GD.Load<PackedScene>(item.Unit.ToString());
				AddToSpawningList(enemyInstance);
			}
		}

	}


	private void AddToSpawningList(PackedScene enemy)
	{
		spawningList.Add(enemy);
	}


	private void SpawnListedEnemy()
	{
		if (spawningList.Count == 0) return;

		var randomNumberGenerator = new RandomNumberGenerator();

		int itemIndex = randomNumberGenerator.RandiRange(0, spawningList.Count - 1);
		var item = spawningList[itemIndex];
		int enemyCount = 0;

		var spawner = WaveSpawner[randomNumberGenerator.RandiRange(0, WaveSpawner.Count - 2)] as Node2D;

		var enemy = spawner.GetNode<Area2D>("./Area2D").GetOverlappingAreas();

		foreach (var area in enemy)
		{
			if (area.HasMeta("Enemy"))
				enemyCount += 1;
		}

		if (enemyCount > 0) return;

		var instance = item.Instantiate();
		var body = instance as CharacterBody2D;
		body.GlobalPosition = spawner.GlobalPosition;
		spawningList.RemoveAt(itemIndex);
		enemyNode.AddChild(body);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (dontSpawnWave) return;

		if (enemyNode.GetChildCount() <= 0 && spawningList.Count == 0)
		{
			EndWave();
		}
	}
}
