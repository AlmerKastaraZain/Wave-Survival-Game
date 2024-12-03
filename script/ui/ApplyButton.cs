using Godot;
using System;

public partial class ApplyButton : Button
{
	private SettingsController settingsController;
	private Slider masterVolumeSlider;
	private Slider sfxVolumeSlider;
	private Slider musicVolumeSlider;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		settingsController = GetParent<SettingsController>();

		masterVolumeSlider = GetNode<Slider>("../MasterSlider/HSlider");
		sfxVolumeSlider = GetNode<Slider>("../SFXSlider/HSlider");
		musicVolumeSlider = GetNode<Slider>("../MusicSlider/HSlider");
		this.Pressed += ApplySettings;

	}

	private void ApplySettings()
	{
		settingsController.SaveSettings(
			masterVolumeSlider.Value,
			sfxVolumeSlider.Value,
			musicVolumeSlider.Value
		);

	}
}
