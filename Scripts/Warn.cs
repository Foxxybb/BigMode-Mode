using Godot;
using System;

public partial class Warn : AnimatedSprite2D
{
	float fadeTick = 4f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.SelfModulate = new Color(1, 1, 1, fadeTick);

		fadeTick -= 0.1f;

		if (fadeTick <= 0){
			this.QueueFree();
		}
	}
}
