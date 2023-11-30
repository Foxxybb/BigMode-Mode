using Godot;
using System;

public partial class Bullet : Node2D
{
	public Vector2 bulletVector = new Vector2(0,0); // used to determine direction of bullet
	public float bulletSpeed = 15;
	public float bulletVariance = 0.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Position = this.Position + bulletVector*bulletSpeed;
	}
}
