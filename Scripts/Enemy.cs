using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	int HP = 30;
	Vector2 movementVec;

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
		

		Velocity = movementVec;
		MoveAndSlide();
	}

	public void TakeDamage(){
		HP--;
		flash = 1;
	}

	
}
