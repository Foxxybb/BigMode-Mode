using Godot;
using System;

public partial class EnemyG : CharacterBody2D
{
	int stun = 3;
	int stunTick;

	public Vector2 movementVec;
	public float moveSpeed = 100;
	public int HP = 18;

	ShaderMaterial shader;
	float flash;

	Player player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//shader = (ShaderMaterial)GetNode<TextureRect>("TextureRect").Material;
		shader = (ShaderMaterial)GetNode<AnimatedSprite2D>("AnimatedSprite2D").Material;

		// get player node to chase
		player = GetNode<Player>("/root/Scene/Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		shader.SetShaderParameter("flash_modifier", flash);
		if (flash > 0) flash -= 0.25f;
	}

	public override void _PhysicsProcess(double delta)
	{
		// Movement
		try {
			if (player.GlobalPosition.X < this.GlobalPosition.X){
			movementVec = new Vector2(-moveSpeed,0);
			} else if (player.GlobalPosition.X > this.GlobalPosition.X){
				movementVec = new Vector2(moveSpeed,0);
			}
		} catch {
			// player is dead
			movementVec = Vector2.Zero;
		}

		// HP
		if (HP <= 0){

			// spawn particles
			GpuParticles2D KP = (GpuParticles2D)Database.Instance.killParticles.Instantiate();
			GetNode<Node2D>("/root/Scene").AddChild(KP);
			KP.GlobalPosition = this.GlobalPosition;
			KP.Emitting = true;

			SoundManager.Instance.PlaySoundAtNode(SoundManager.Instance.kill, this, 5);
			this.QueueFree();
		}

		// Stun
		if (stunTick > 0){
			Velocity = Vector2.Zero;
			stunTick--;
		} else {
			Velocity = movementVec;
		}
		
		MoveAndSlide();
		CheckBodyCollisions();
	}

	void CheckBodyCollisions()
	{
		// check body collisions
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var coll = GetSlideCollision(i);
			var body = coll.GetCollider();
			//GD.Print("Collided with: " + body.GetType().ToString());

			// if collision with "Enemy", player death
			if (!player.markedForDeath){
				if ((body.GetType().ToString() == "Player")){
					GD.Print("death");
					player.PlayerDeath();
				}
			}
		}
	}

	public void TakeDamage(){
		HP--;
		stunTick = stun;
		flash = 1;
	}

}
