using Godot;
using System;

public partial class ContinueButton : Button
{
	private GameMenu gameMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameMenu = GetParent<Panel>().GetParent<GameMenu>();

		this.Pressed += ContinueGame;
		
	}

	private void ContinueGame()
	{
		gameMenu.HideMenu();
	}

}
