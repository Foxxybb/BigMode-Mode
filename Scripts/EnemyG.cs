using Godot;
using System;

public partial class EnemyG : CharacterBody2D
{
	int stun = 3;
	int stunTick;

	public Vector2 movementVec;
	public float moveSpeed = 60;
	public int HP = 20;

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
			SoundManager.Instance.PlaySoundAtNode(SoundManager.Instance.kill, this, 0);
			this.QueueFree();
		}

		shader.SetShaderParameter("flash_modifier", flash);
		if (flash > 0) flash -= 0.25f;
	}

	public override void _PhysicsProcess(double delta)
	{

		// stun
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
