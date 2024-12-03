using Godot;
using System;

public partial class FXAudioStreamPlayer : AudioStreamPlayer2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SystemVariable systemVariable = GetNode<SystemVariable>("/root/SystemVariable");

		this.VolumeDb = Mathf.LinearToDb((float)systemVariable.GetMasterVolume() * (float)systemVariable.GetSFXVolume() / 1000);
	}
}
