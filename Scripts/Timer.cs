using Godot;
using System;

public partial class Timer : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Text = Math.Round(Oracle.Instance.time, 2).ToString();
		//this.Text = Oracle.Instance.time.ToString();
	}
}
