using Godot;
using System;

public partial class GoBackButton : Button
{
	private MenuController menuController;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Pressed += GoToSettings;
		menuController = GetTree().Root.GetNode<MenuController>("Menu");
	}

	private void GoToSettings()
	{
		menuController.ChangeCurrentState(MenuController.Menu.MainMenu);
	}
}
