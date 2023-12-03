using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	int stun = 3;
	int stunTick;

	public Vector2 movementVec;
	public int HP = 30;

	ShaderMaterial shader;
	float flash;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shader = (ShaderMaterial)GetNode<TextureRect>("TextureRect").Material;

		movementVec = new Vector2(-50,0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		// HP
		if (HP <= 0){
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
	}

	public void TakeDamage(){
		HP--;
		stunTick = stun;
		flash = 1;
	}

	
}
