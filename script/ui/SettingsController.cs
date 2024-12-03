using Godot;
using System;

public partial class SettingsController : Control
{
	SystemVariable systemVariables;
	MenuController controller;
	public override void _Ready()
	{
		systemVariables = GetNode<SystemVariable>("/root/SystemVariable");

		controller = GetParent<MenuController>();
		controller.GoToSettings += SetSettingsToSystemVariables;
	}

	private void SetSettingsToSystemVariables()
	{
		GetNode<Slider>("MasterSlider/HSlider").Value = systemVariables.GetMasterVolume();
		GetNode<Slider>("SFXSlider/HSlider").Value = systemVariables.GetSFXVolume();
		GetNode<Slider>("MusicSlider/HSlider").Value = systemVariables.GetMusicVolume();
	}

	public void SaveSettings(double masterVolume, double SFXVolume, double musicVolume)
	{
		systemVariables.changeMasterVolume(masterVolume);
		systemVariables.changeSFXVolume(SFXVolume);
		systemVariables.changeMusicVolume(musicVolume);

		GetTree().ReloadCurrentScene();
	}
}
