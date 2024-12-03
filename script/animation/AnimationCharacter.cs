using Godot;
using System;
using System.Diagnostics;

public partial class AnimationCharacter : AnimationPlayer
{
	// Character's animation state
	private class PlayerAnimationState
	{
		public const string Idle_Up = "Up_Idle_Animation";
		public const string Idle_Down = "Down_Idle_Animation";
		public const string Idle_Left = "Left_Idle_Animation";
		public const string Idle_Right = "Right_Idle_Animation";
		public const string Up = "Up_Animation";
		public const string Down = "Down_Animation";
		public const string Left = "Left_Animation";
		public const string Right = "Right_Animation";
	}

	private string currentDirection = PlayerAnimationState.Right;
	private AnimationPlayer animationPlayer;
	private CharacterBody2D parent;
	[Export] private float defaultAnimationSpeed;
	private float animationSpeed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationPlayer = this;
		parent = GetParent<CharacterBody2D>();
		SetAnimationSpeedToDefault();
	}

	private double getDegree(double angle)
	{
		return angle * MathF.PI / 180;
	}

	public void SpeedUpAnimation(float speed)
	{
		animationPlayer.SpeedScale = 4;
	}

	public void SetAnimationSpeedToDefault()
	{
		animationPlayer.SpeedScale = defaultAnimationSpeed;
	}

	public void SlowDownAnimationSpeed(float speed)
	{
		animationPlayer.SpeedScale = 1;
	}

	public void SetIdleAnimationOnPreviousDirection()
	{
		/// If the player is not moving, it plays the corresponding idle animation.//+

		/// If the player is moving, it plays the corresponding movement animation.//+
		switch (currentDirection)
		{
			case PlayerAnimationState.Up:
				currentDirection = PlayerAnimationState.Idle_Up;
				animationPlayer.Play(PlayerAnimationState.Idle_Up);
				break;
			case PlayerAnimationState.Down:
				currentDirection = PlayerAnimationState.Idle_Down;
				animationPlayer.Play(PlayerAnimationState.Idle_Down);
				break;
			case PlayerAnimationState.Right:
				currentDirection = PlayerAnimationState.Idle_Right;
				animationPlayer.Play(PlayerAnimationState.Idle_Right);
				break;
			case PlayerAnimationState.Left:
				currentDirection = PlayerAnimationState.Idle_Left;
				animationPlayer.Play(PlayerAnimationState.Idle_Left);
				break;
		}
	}

	public void SetIdleAnimation(double Angle)
	{
		/// If the player is not moving, it plays the corresponding idle animation.//+

		/// If the player is moving, it plays the corresponding movement animation.//+
		if (Angle < getDegree(45) && Angle > -getDegree(45))
		{
			currentDirection = PlayerAnimationState.Idle_Right;
			animationPlayer.Play(currentDirection);
		}
		else if (Angle <= -getDegree(45) && Angle >= -getDegree(135))
		{
			currentDirection = PlayerAnimationState.Idle_Up;
			animationPlayer.Play(currentDirection);
		}
		else if (Angle >= getDegree(45) && Angle <= getDegree(135))
		{
			currentDirection = PlayerAnimationState.Idle_Down;
			animationPlayer.Play(currentDirection);
		}
		else
		{
			currentDirection = PlayerAnimationState.Idle_Left;
			animationPlayer.Play(currentDirection);
		}
	}

	public void SetWalkingAnimation(double Angle)
	{
		/// If the player is moving, it plays the corresponding movement animation.//+
		if (Angle < getDegree(45) && Angle > -getDegree(45))
		{
			currentDirection = PlayerAnimationState.Right;
			animationPlayer.Play(currentDirection);
		}
		else if (Angle <= -getDegree(45) && Angle >= -getDegree(135))
		{
			currentDirection = PlayerAnimationState.Up;
			animationPlayer.Play(currentDirection);
		}
		else if (Angle >= getDegree(45) && Angle <= getDegree(135))
		{
			currentDirection = PlayerAnimationState.Down;
			animationPlayer.Play(currentDirection);
		}
		else
		{
			currentDirection = PlayerAnimationState.Left;
			animationPlayer.Play(currentDirection);
		}
	}
}
