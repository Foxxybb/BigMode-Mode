using Godot;
using System;

public partial class AudioShot : AudioStreamPlayer2D
{
	private void _on_finished()
	{
		this.QueueFree();
	}
}



