using Godot;
using System;

public partial class KnifeAnimationPlayer : AnimationPlayer
{
	public void PlaySwingAnimation()
	{
		Play("Swing");
	}
}
