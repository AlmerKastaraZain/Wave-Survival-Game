using Godot;
using System;

public partial class ControlGuideButton : Button
{
	private MenuController menuController;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Pressed += GoToControlGuide;
		menuController = GetTree().Root.GetNode<MenuController>("Menu");
	}

	private void GoToControlGuide()
	{
		menuController.ChangeCurrentState(MenuController.Menu.Control);
	}
}
