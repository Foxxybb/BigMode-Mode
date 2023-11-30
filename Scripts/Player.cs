using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	// used for bullet spawns
	Node2D leftArm;
	Node2D rightArm;
	int altInt = 1;

	public override void _Ready(){
		leftArm = GetNode<Node2D>("LeftArm");
		rightArm = GetNode<Node2D>("RightArm");
	}

	public override void _Process(double delta){

		// shoot left
		if (Input.IsActionJustPressed("shoot_left")){
			ShootBulletHorizontal(-1);
		}
		// shoot right
		if (Input.IsActionJustPressed("shoot_right")){
			ShootBulletHorizontal(1);
		}
	}

	public override void _PhysicsProcess(double delta)
	{

		Vector2 velocity = Velocity;
		
		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	void ShootBulletHorizontal(int dir){
		
		// spawn bullet
		Bullet newBullet = (Bullet)Database.Instance.bullet.Instantiate();
		GetNode<Node2D>("/root/Scene").AddChild(newBullet);

		// set position of bullet
		if (dir == 1) { newBullet.Position = rightArm.GlobalPosition; }
		else { newBullet.Position = leftArm.GlobalPosition; }
		
		// set vector of bullet
		newBullet.bulletVector = new Vector2(dir, 0);
		
		//newBullet.bulletVector = new Vector2(dir, (float)(GD.Randf()*(0.2)*altInt));
		//altInt *= -1;
	}
}
