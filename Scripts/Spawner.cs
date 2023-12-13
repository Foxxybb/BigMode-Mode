using Godot;
using System;
using System.Collections.Generic;

public partial class Spawner : Node2D
{
	Node2D spawn1;
	Node2D spawn2;
	Node2D spawn3;
	Node2D spawn4;
	Node2D spawn5;
	Node2D spawn6;
	Node2D spawn7;
	Node2D spawn8;
	Node2D spawn9;

	List<Node2D> spawnList = new List<Node2D>();

	Node2D warn1;
	Node2D warn2;
	Node2D warn3;
	Node2D warn4;
	Node2D warn5;
	Node2D warn6;
	Node2D warn7;
	Node2D warn8;
	Node2D warn9;

	List<Node2D> warnList = new List<Node2D>();

	int eventTick = 0; // keeps track of current spawn event
	bool bigRightStart;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// spawns
		spawn1 = GetNode<Node2D>("Spawn1");
		spawn2 = GetNode<Node2D>("Spawn2");
		spawn3 = GetNode<Node2D>("Spawn3");
		spawn4 = GetNode<Node2D>("Spawn4");
		spawn5 = GetNode<Node2D>("Spawn5");
		spawn6 = GetNode<Node2D>("Spawn6");
		spawn7 = GetNode<Node2D>("Spawn7");
		spawn8 = GetNode<Node2D>("Spawn8");
		spawn9 = GetNode<Node2D>("Spawn9");

		spawnList.Add(spawn1);
		spawnList.Add(spawn2);
		spawnList.Add(spawn3);
		spawnList.Add(spawn4);
		spawnList.Add(spawn5);
		spawnList.Add(spawn6);
		spawnList.Add(spawn7);
		spawnList.Add(spawn8);
		spawnList.Add(spawn9);

		// warns
		warn1 = GetNode<Node2D>("Warn1");
		warn2 = GetNode<Node2D>("Warn2");
		warn3 = GetNode<Node2D>("Warn3");
		warn4 = GetNode<Node2D>("Warn4");
		warn5 = GetNode<Node2D>("Warn5");
		warn6 = GetNode<Node2D>("Warn6");
		warn7 = GetNode<Node2D>("Warn7");
		warn8 = GetNode<Node2D>("Warn8");
		warn9 = GetNode<Node2D>("Warn9");

		warnList.Add(warn1);
		warnList.Add(warn2);
		warnList.Add(warn3);
		warnList.Add(warn4);
		warnList.Add(warn5);
		warnList.Add(warn6);
		warnList.Add(warn7);
		warnList.Add(warn8);
		warnList.Add(warn9);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	void SpawnEnemy(int spawnIdx, int type){
		
		// spawn enemy by type
		switch (type){
			case 0: // ground
				EnemyG newEnemyG = (EnemyG)Database.Instance.enemyG.Instantiate();
				GetNode<Node2D>("/root/Scene").AddChild(newEnemyG);
				newEnemyG.GlobalPosition = spawnList[spawnIdx].Position;
				break;
			case 1: // air
				EnemyA newEnemyA = (EnemyA)Database.Instance.enemyA.Instantiate();
				GetNode<Node2D>("/root/Scene").AddChild(newEnemyA);
				newEnemyA.GlobalPosition = spawnList[spawnIdx].Position;
				break;
			case 2: // big
				EnemyB newEnemyB = (EnemyB)Database.Instance.enemyB.Instantiate();
				GetNode<Node2D>("/root/Scene").AddChild(newEnemyB);
				newEnemyB.GlobalPosition = spawnList[spawnIdx].Position;
				break;
			case 3: // final
				EnemyF newEnemyF = (EnemyF)Database.Instance.enemyF.Instantiate();
				GetNode<Node2D>("/root/Scene").AddChild(newEnemyF);
				newEnemyF.GlobalPosition = spawnList[spawnIdx].Position;
				break;
			default:
			break;
		}

		// trigger warning
		Warn newWarn = (Warn)Database.Instance.warn.Instantiate();
		this.AddChild(newWarn);
		newWarn.GlobalPosition = warnList[spawnIdx].GlobalPosition;

		if (type > 1){
			// trigger BIG warning sound
			SoundManager.Instance.PlaySound(SoundManager.Instance.aggro);
			// scale up warn
			newWarn.Scale = new Vector2(6,6);
			newWarn.fadeTick = 7;
		} else {
			// normal sound
			SoundManager.Instance.PlaySound(SoundManager.Instance.warn);
		}
	}

	void SpawnGround(){
		SpawnEnemy(0,0);
		SpawnEnemy(3,0);
	}

	void SpawnAir(){
		SpawnEnemy(1,1);
		SpawnEnemy(2,1);
		SpawnEnemy(4,1);
		SpawnEnemy(5,1);
	}

	void SpawnAirLeft(){
		SpawnEnemy(4,1);
		SpawnEnemy(5,1);
	}

	void SpawnAirRight(){
		SpawnEnemy(1,1);
		SpawnEnemy(2,1);
	}

	void SpawnMid(){
		SpawnEnemy(1,1);
	}

	void SpawnTop(){
		SpawnEnemy(2,1);
		SpawnEnemy(5,1);
	}

	void SpawnBigLeft(){
		SpawnEnemy(6,2);
	}

	void SpawnBigRight(){
		SpawnEnemy(7,2);
	}

	void SpawnFinal(){
		SpawnEnemy(8,3);
	}

	private void _on_event_timer_timeout()
	{
		if (!Oracle.Instance.playerDead){
			// trigger next event in sequence
			eventTick++;
			SpawnEvent(eventTick);
		}
	}

	void SpawnEvent(int tick){

		switch (tick) {
			case 1:
				SpawnEnemy(0,0);
				break;
			case 3:
				SpawnEnemy(4,1);
				break;
			case 5:
				SpawnEnemy(2,1);
				break;
			case 8:
				SpawnGround();
				break;
			case 10:
				SpawnAirLeft();
				break;
			case 15:
				
				break;
			case 18:
				SpawnAirRight();
				break;
			case 20:
				
				break;
			case 25:
				SpawnGround();
				break;
			case 30:
				SpawnGround();
				break;
			case 32:
				SpawnMid();
				break;
			case 35:
				SpawnTop();
				break;
			case 40:
				SpawnGround();
				break;
			case 45:
				SpawnAir();
				break;
			case 50:
				SpawnGround();
				break;
			case 55:
				break;
			case 60:
				// spawn on side that player isn't on
				if (GetNode<Player>("/root/Scene/Player").Position.X > 640){
					SpawnBigLeft();
				} else {
					SpawnBigRight();
					bigRightStart = true;
				}
				break;
			case 65:
				if (bigRightStart){
					SpawnBigLeft();
				} else {
					SpawnBigRight();
				}
				break;
			case 70:
				if (bigRightStart){
					SpawnBigRight();
				} else {
					SpawnBigLeft();
				}
				break;
			case 80:
				SpawnFinal();
				break;
			default:
				break;
		}

	}
	
}



