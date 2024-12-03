using Godot;
using System;

public partial class MenuController : Control
{
	public enum Menu
	{
		MainMenu,
		Settings,
		Control
	}

	private Godot.Control MainMenu;
	private Godot.Control Settings;
	private Godot.Control ControlGuide;

	public Action GoToMainMenu;
	public Action GoToSettings;
	public Action GoToControlGuide;
	Menu currentState = Menu.MainMenu;

	public override void _Ready()
	{
		MainMenu = GetNode<Godot.Control>("./MainMenu");
		Settings = GetNode<Godot.Control>("./Settings");
		ControlGuide = GetNode<Godot.Control>("./Control");

		GoToMainMenu += ShowMainMenu;
		GoToSettings += ShowSettings;
		GoToControlGuide += ShowControl;
	}
	// Called when the node enters the scene tree for the first time.
	public void ChangeCurrentState(Menu menu)
	{
		currentState = menu;
		ChangeMenu();
	}

	private void ChangeMenu()
	{
		switch (currentState)
		{
			case Menu.MainMenu:
				GoToMainMenu.Invoke();
				break;
			case Menu.Settings:
				GoToSettings.Invoke();
				break;
			case Menu.Control:
				GoToControlGuide.Invoke();
				break;
			default:
				throw new Exception("Unknown state");
		}
	}

	private void ShowMainMenu()
	{
		MainMenu.Show();
		Settings.Hide();
		ControlGuide.Hide();
	}

	private void ShowSettings()
	{
		MainMenu.Hide();
		Settings.Show();
		ControlGuide.Hide();
	}
	private void ShowControl()
	{
		MainMenu.Hide();
		Settings.Hide();
		ControlGuide.Show();
	}
}

