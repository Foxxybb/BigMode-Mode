using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Drag = 3f;
	public const float AirDrag = 1f;
	public const float JumpVelocity = -500.0f;
	int horSpeedCap = 500;
	int verSpeedCap = 600;
	int bulletForce = 50;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	bool jumpMode = false;
	int modeChangeCooldown = 40;
	int modeChangeCooldownTick;

	// used for bullet spawns
	int baseCoolDown = 5; // cooldown for bullet fire

	Node2D leftArm;
	int leftArmCD = 0; // cooldown

	Node2D rightArm;
	int rightArmCD = 0;

	AnimatedSprite2D an;
	public string anState;
	const string IDLE = "idle";
	const string AIR = "air";
	const string JUMP = "jump";
	const string FIRE = "fire";


	public override void _Ready()
	{
		an = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		leftArm = GetNode<Node2D>("LeftArm");
		rightArm = GetNode<Node2D>("RightArm");

		ChangeAnimationState(IDLE);
	}

	public override void _Process(double delta)
	{
		if (modeChangeCooldownTick <= 0)
		{
			// shoot left
			if (Input.IsActionPressed("shoot_left"))
			{
				// if gun is cool, shoot and reset CD
				if (leftArmCD <= 0)
				{
					if (jumpMode){
						ShootBulletDown(-1);
						// apply bullet force
						Velocity = Velocity + new Vector2(bulletForce/(1.5f), -bulletForce*2);
					} else {
						ShootBulletHorizontal(-1);
						// apply bullet force
						Velocity = Velocity + new Vector2(bulletForce, 0);
					}
					
					leftArmCD = baseCoolDown;
				}
				else
				{ // tick down CD
					leftArmCD--;
				}
			}

			// shoot right
			if (Input.IsActionPressed("shoot_right"))
			{
				// if gun is cool, shoot and reset CD
				if (rightArmCD <= 0)
				{
					if (jumpMode){
						ShootBulletDown(1);
						// apply bullet force
						Velocity = Velocity + new Vector2(-bulletForce/(1.5f), -bulletForce*2);
					} else {
						ShootBulletHorizontal(1);
						// apply bullet force
						Velocity = Velocity + new Vector2(-bulletForce, 0);
					}
					
					rightArmCD = baseCoolDown;
				}
				else
				{ // tick down CD
					rightArmCD--;
				}
			}
		} else {
			// tick down modeChangeCooldownTick
			modeChangeCooldownTick--;
		}

		HandleAnimation();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			if (jumpMode){
				velocity.Y += gravity * (float)delta;
			} else {
				velocity.Y += gravity/5 * (float)delta;
			}
			
		}
			
		// Handle Jump.
		if (Input.IsActionJustPressed("mode_change") && modeChangeCooldownTick <= 0)
		{
			// go up when switching to jumpmode, fall when switching to firemode
			if (!jumpMode){
				velocity.Y = JumpVelocity;
			} else {
				//velocity.Y = -JumpVelocity;
			}

			// mode change
			ModeChange();
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		//Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		// if (direction != Vector2.Zero)
		// {
		// 	velocity.X = direction.X * Speed;
		// }
		// else
		// {
		//velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		// }

		if (IsOnFloor()){
			velocity.X = Mathf.MoveToward(velocity.X, 0, Drag);
		} else {
			velocity.X = Mathf.MoveToward(velocity.X, 0, AirDrag);
		}

		// clamp speed
		velocity.X = Math.Clamp(velocity.X, -horSpeedCap, horSpeedCap);
		velocity.Y = Math.Clamp(velocity.Y, -verSpeedCap+100, verSpeedCap+200);

		Velocity = velocity;
		MoveAndSlide();

		CheckBodyCollisions();
	}

	void CheckBodyCollisions(){
		// check body collisions
		for (int i = 0; i < GetSlideCollisionCount(); i++){
			var coll = GetSlideCollision(i);
			var body = coll.GetCollider();
			//GD.Print("Collided with: " + body.GetType().ToString());

			// if collision with "Enemy", player death
			if (body.GetType().ToString() == "Enemy"){
				GD.Print("death");
				PlayerDeath();
			}
		}
	}

	void PlayerDeath(){
		this.QueueFree();
		// display retry message
		GetNode<Control>("/root/Scene/Control").Visible = true;
	}

	void ShootBulletHorizontal(int dir)
	{
		// spawn bullet
		Bullet newBullet = (Bullet)Database.Instance.bullet.Instantiate();
		GetNode<Node2D>("/root/Scene").AddChild(newBullet);

		// set position of bullet
		if (dir == 1) { newBullet.Position = rightArm.GlobalPosition; }
		else { newBullet.Position = leftArm.GlobalPosition; }

		// set vector of bullet with variance
		newBullet.bulletVector = new Vector2(dir, (float)GD.RandRange(-0.1, 0.1));
	}

	void ShootBulletDown(int dir){
		// spawn bullet
		Bullet newBullet = (Bullet)Database.Instance.bullet.Instantiate();
		GetNode<Node2D>("/root/Scene").AddChild(newBullet);

		// set position of bullet
		if (dir == 1) { newBullet.Position = rightArm.GlobalPosition; }
		else { newBullet.Position = leftArm.GlobalPosition; }

		// set vector of bullet with variance
		newBullet.bulletVector = new Vector2((float)GD.RandRange(-0.1, 0.1), 1);
	}

	void ModeChange()
	{
		// briefly remove control from the player with an Integer
		modeChangeCooldownTick = modeChangeCooldown;

		// move arm cannon positions
		// if in jumpMode, move cannons back
		if (jumpMode){
			leftArm.Position = leftArm.Position + new Vector2(-28,-32);
			rightArm.Position = rightArm.Position + new Vector2(28,-32);

			// play animation
			ChangeAnimationState(FIRE);
		} else {
			leftArm.Position = leftArm.Position + new Vector2(28,32);
			rightArm.Position = rightArm.Position + new Vector2(-28,32);

			// play animation
			ChangeAnimationState(JUMP);
		}

		jumpMode = !jumpMode;
	}

	// function to manually switch animation states
	public void ChangeAnimationState(string newState){
		//stop self interruption
		if (anState == newState){
			// switch statement contains all animations that can cancel into themselves
			switch (newState){
				default:
					return;
			}
		}

		// play animation
		an.Play(newState);

		// assign state
		anState = newState;
	}

	// function to automatically transition animations
	void HandleAnimation(){

		// auto transitions (on animation end)
		if (!an.IsPlaying())
		{	
			switch (anState){
				case JUMP:
					ChangeAnimationState(AIR);
					break;
				case FIRE:
					ChangeAnimationState(IDLE);
					break;
				default:
					break;
			}
		} else {
			// IsPlaying
			switch (anState){
				default:
					break;
			}
		}
	}
}
