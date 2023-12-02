using Godot;
using System;

public partial class MyCamera : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//create transition child node
		this.AddChild(Database.Instance.transition.Instantiate());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
