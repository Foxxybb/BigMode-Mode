using Godot;
using System;

public partial class Bullet : Node2D
{
	public Vector2 bulletVector = new Vector2(0, 0); // used to determine direction of bullet
	public float bulletSpeed = 15;
	public float bulletVariance = 0.0f;

	int lifeSpan = 180;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// move bullet
		this.Position = this.Position + bulletVector * bulletSpeed;

		// tickdown lifespawn
		lifeSpan--;
		if (lifeSpan <= 0)
		{
			this.QueueFree();
		}
	}

	private void _on_area_2d_body_entered(Node2D body)
	{
		//GD.Print(body.GetType().ToString());
		
		switch (body.GetType().ToString())
			{
				case "Godot.StaticBody2D":
					Collide();
					break;
				case "Enemy":
					Enemy enemy = (Enemy)body; // get enemy script
					enemy.TakeDamage();
					Collide();
					break;
				default:
				break;
			}
	}

	void Collide(){
		this.QueueFree();
	}

}






