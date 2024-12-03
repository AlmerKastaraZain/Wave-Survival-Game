using Godot;
using System;

public partial class CameraShake : Camera2D
{
	[Export] float randomStrength = 4f;
	[Export] float shakeFade = 10.0f;
	private RandomNumberGenerator rng = new RandomNumberGenerator();

	private float shakeStrength = 0.0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	public void ApplyShake()
	{
		shakeStrength = randomStrength;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (shakeStrength > 0)
		{
			shakeStrength = Mathf.Lerp(shakeStrength, 0, shakeFade * (float)delta);

			Offset = RandomOffset();
		}
	}

	private Vector2 RandomOffset()
	{
		return new Vector2(rng.RandfRange(-shakeStrength, shakeStrength), rng.RandfRange(-shakeStrength, shakeStrength));
	}
}
