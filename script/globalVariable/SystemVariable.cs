using Godot;
using System;

public partial class SystemVariable : Node
{
	private double musicVolume = 50f;
	private double masterVolume = 100f;
	private double sfxVolume = 50f;

	public void changeMusicVolume(double volume)
	{
		musicVolume = volume;
	}

	public void changeMasterVolume(double volume)
	{
		masterVolume = volume;
	}

	public void changeSFXVolume(double volume)
	{
		sfxVolume = volume;
	}

	public double GetMusicVolume()
	{
		return musicVolume;
	}

	public double GetMasterVolume()
	{
		return masterVolume;
	}

	public double GetSFXVolume()
	{
		return sfxVolume;
	}

}
