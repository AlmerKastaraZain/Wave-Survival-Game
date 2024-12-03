using Godot;
using System;


public class AudioData
{
	public AudioStreamWav Audio = new AudioStreamWav();
	public AudioData(AudioStreamWav audio)
	{
		Audio = audio;
	}
}