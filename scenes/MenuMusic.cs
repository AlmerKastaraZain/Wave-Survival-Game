using Godot;
using System;

public partial class MenuMusic : AudioStreamPlayer2D
{


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SystemVariable systemVariable = GetNode<SystemVariable>("/root/SystemVariable");
		this.VolumeDb = Mathf.LinearToDb((float)systemVariable.GetMasterVolume() * (float)systemVariable.GetMusicVolume() / 1000);

		// Connect to the "level_finished" signal
		this.Finished += replayAudio;
	}

	private void replayAudio()
	{
		this.Play();
	}

}
