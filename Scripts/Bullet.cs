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
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta){

		// move bullet
		this.Position = this.Position + bulletVector * bulletSpeed;

		// tickdown lifespawn
		lifeSpan--;
		if (lifeSpan <= 0)
		{
			OnDestroy();
			this.QueueFree();
		}
		
	}

	private void _on_area_2d_body_entered(Node2D body)
	{
		//GD.Print(body.GetType().ToString());

		switch (body.GetType().ToString())
		{
			case "Godot.StaticBody2D":
				this.QueueFree();
				break;
			case "GroundB":
				Collide();
				break;
			case "Enemy":
				Enemy enemy = (Enemy)body; // get enemy script
				enemy.TakeDamage();
				SoundManager.Instance.PlaySoundAtNode(SoundManager.Instance.hit, body, -1);
				Collide();
				break;
			case "EnemyG":
				EnemyG enemyG = (EnemyG)body; // get enemy script
				enemyG.TakeDamage();
				SoundManager.Instance.PlaySoundAtNode(SoundManager.Instance.hit, body, 0);
				Collide();
				break;
			case "EnemyA":
				EnemyA enemyA = (EnemyA)body; // get enemy script
				enemyA.TakeDamage();
				SoundManager.Instance.PlaySoundAtNode(SoundManager.Instance.hit, body, 0);
				Collide();
				break;
			case "EnemyB":
				EnemyB enemyB = (EnemyB)body; // get enemy script
				enemyB.TakeDamage();
				SoundManager.Instance.PlaySoundAtNode(SoundManager.Instance.hit, body, -2, 0.9f);
				Collide();
				break;
			case "EnemyF":
				EnemyF enemyF = (EnemyF)body; // get enemy script
				enemyF.TakeDamage();
				SoundManager.Instance.PlaySoundAtNode(SoundManager.Instance.hit, body, -3, 0.9f);
				Collide();
				break;
			default:
				
				break;
		}
	}

	void Collide()
	{
		OnDestroy();
		this.QueueFree();
	}

	void OnDestroy(){
		GpuParticles2D KP = (GpuParticles2D)Database.Instance.bulletParticles.Instantiate();
		GetNode<Node2D>("/root/Scene").AddChild(KP);
		KP.GlobalPosition = this.GlobalPosition;
		KP.Emitting = true;
	}

}






