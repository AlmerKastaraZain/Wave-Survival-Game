using Godot;
using System;

public partial class GameMenu : Control
{
	bool MenuIsActive = false;

	public override void _Ready()
	{
	}


	public void ShowMenu()
	{
		this.Show();
		GetTree().Paused = true;
	}

	public void HideMenu()
	{
		this.Hide();
		GetTree().Paused = false;
	}

	public void ToggleMenu()
	{
		if (!MenuIsActive)
		{
			MenuIsActive = true;
			ShowMenu();
			return;
		}

		MenuIsActive = false;
		HideMenu();
	}

	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Escape))
			ToggleMenu();

	}
}
