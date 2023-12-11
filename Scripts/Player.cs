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

	public bool markedForDeath = false;
	bool jumpMode = false;
	int modeChangeCooldown = 40;
	int modeChangeCooldownTick;

	// used for bullet spawns
	int baseCoolDown = 5; // cooldown for bullet fire

	Node2D leftArm;
	int leftArmCD = 0; // cooldown
	GpuParticles2D leftArmFlash;

	Node2D rightArm;
	int rightArmCD = 0;
	GpuParticles2D rightArmFlash;

	AudioStreamPlayer2D gunSoundPlayer;

	AnimatedSprite2D an;
	public string anState;
	const string IDLE = "idle";
	const string AIR = "air";
	const string JUMP = "jump";
	const string FIRE = "fire";

	public override void _Ready()
	{
		an = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		gunSoundPlayer = GetNode<AudioStreamPlayer2D>("GunSoundPlayer");
		gunSoundPlayer.Stream = SoundManager.Instance.shot;

		leftArm = GetNode<Node2D>("LeftArm");
		leftArmFlash = leftArm.GetNode<GpuParticles2D>("LeftGunFlash");
		rightArm = GetNode<Node2D>("RightArm");
		rightArmFlash = rightArm.GetNode<GpuParticles2D>("RightGunFlash");

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

		if (Input.IsActionJustReleased("shoot_right")){
			rightArmCD = 0;
		}
		if (Input.IsActionJustReleased("shoot_left")){
			leftArmCD = 0;
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
				// smaller jump into firemode
				velocity.Y = JumpVelocity/4;
			}

			// mode change
			ModeChange();
		}

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
			// if (!markedForDeath){
			// 	if ((body.GetType().ToString() == "EnemyG") || (body.GetType().ToString() == "EnemyA")){
			// 		GD.Print("death");
			// 		PlayerDeath();
			// 	}
			// }
			
		}
	}

	public void PlayerDeath(){
		markedForDeath = true;
		Oracle.Instance.playerDead = true;

		// spawn particles
		GpuParticles2D DP = (GpuParticles2D)Database.Instance.deathParticles.Instantiate();
		GetNode<Node2D>("/root/Scene").AddChild(DP);
		DP.GlobalPosition = this.GlobalPosition;
		DP.Emitting = true;

		// play death sound
		SoundManager.Instance.PlaySoundAtNode(SoundManager.Instance.death, this, 3);

		// stop music
		SoundManager.Instance.StopMusic();

		// update PB time
		if (Oracle.Instance.PBTime > Oracle.Instance.displayTime){
			Oracle.Instance.PBTime = Oracle.Instance.displayTime;
			if (Oracle.Instance.PBTime < 0){
				Oracle.Instance.PBTime = 0;
			}
		}
		
		this.QueueFree();
		// display retry message
		GetNode<Control>("/root/Scene/RestartText").Visible = true;
		Oracle.Instance.displayTimerOn = false;

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

		// trigger gun flash particles
		if (dir == 1){
			rightArmFlash.Emitting = true;
		} else {
			leftArmFlash.Emitting = true;
		}

		// play sound
		//SoundManager.Instance.PlaySoundOnNode(SoundManager.Instance.shot, this, 1);
		// instead play sound from player gun, so sounds are interrupted
		gunSoundPlayer.PitchScale = 1.0f;
		gunSoundPlayer.Play();
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

		// trigger gun flash particles
		if (dir == 1){
			rightArmFlash.Emitting = true;
		} else {
			leftArmFlash.Emitting = true;
		}

		// play sound
		gunSoundPlayer.PitchScale = 1.0f;
		gunSoundPlayer.Play();
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
			leftArm.RotationDegrees = 0;
			rightArm.RotationDegrees = 0;

			// play animation
			ChangeAnimationState(FIRE);
			// play sound
			SoundManager.Instance.PlaySoundOnNode(SoundManager.Instance.fire, this, 4);
			SoundManager.Instance.PlaySoundOnNode(SoundManager.Instance.voice_fire, this, 6);
			
		} else {
			leftArm.Position = leftArm.Position + new Vector2(28,32);
			rightArm.Position = rightArm.Position + new Vector2(-28,32);
			leftArm.RotationDegrees = -90;
			rightArm.RotationDegrees = 90;

			// play animation
			ChangeAnimationState(JUMP);
			// play sound
			SoundManager.Instance.PlaySoundOnNode(SoundManager.Instance.jump, this, 4);
			SoundManager.Instance.PlaySoundOnNode(SoundManager.Instance.voice_jump, this, 5);
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
