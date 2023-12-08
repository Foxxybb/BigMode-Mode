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

	List<Node2D> spawnList = new List<Node2D>();

	Node2D warn1;
	Node2D warn2;
	Node2D warn3;
	Node2D warn4;
	Node2D warn5;
	Node2D warn6;

	List<Node2D> warnList = new List<Node2D>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawn1 = GetNode<Node2D>("Spawn1");
		spawn2 = GetNode<Node2D>("Spawn2");
		spawn3 = GetNode<Node2D>("Spawn3");
		spawn4 = GetNode<Node2D>("Spawn4");
		spawn5 = GetNode<Node2D>("Spawn5");
		spawn6 = GetNode<Node2D>("Spawn6");

		spawnList.Add(spawn1);
		spawnList.Add(spawn2);
		spawnList.Add(spawn3);
		spawnList.Add(spawn4);
		spawnList.Add(spawn5);
		spawnList.Add(spawn6);

		warn1 = GetNode<Node2D>("Warn1");
		warn2 = GetNode<Node2D>("Warn2");
		warn3 = GetNode<Node2D>("Warn3");
		warn4 = GetNode<Node2D>("Warn4");
		warn5 = GetNode<Node2D>("Warn5");
		warn6 = GetNode<Node2D>("Warn6");

		warnList.Add(warn1);
		warnList.Add(warn2);
		warnList.Add(warn3);
		warnList.Add(warn4);
		warnList.Add(warn5);
		warnList.Add(warn6);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("test_action")){
			GD.Print("spawn");
			SpawnGround();
			SpawnAir();
		}
	}

	void SpawnEnemy(int spawnIdx, string type){
		
		// spawn enemy by type
		switch (type){
			case "ground": // ground
				EnemyG newEnemyG = (EnemyG)Database.Instance.enemyG.Instantiate();
				GetNode<Node2D>("/root/Scene").AddChild(newEnemyG);
				newEnemyG.GlobalPosition = spawnList[spawnIdx].Position;
			break;
			case "air": // air
				EnemyA newEnemyA = (EnemyA)Database.Instance.enemyA.Instantiate();
				GetNode<Node2D>("/root/Scene").AddChild(newEnemyA);
				newEnemyA.GlobalPosition = spawnList[spawnIdx].Position;
			break;
			default:
			break;
		}

		// trigger warning
		Warn newWarn = (Warn)Database.Instance.warn.Instantiate();
		this.AddChild(newWarn);
		newWarn.GlobalPosition = warnList[spawnIdx].GlobalPosition;
	}

	void SpawnGround(){
		SpawnEnemy(0,"ground");
		SpawnEnemy(3,"ground");

		SoundManager.Instance.PlaySound(SoundManager.Instance.warn);
	}

	void SpawnAir(){
		SpawnEnemy(1,"air");
		SpawnEnemy(2,"air");
		SpawnEnemy(4,"air");
		SpawnEnemy(5,"air");
	}
	
}
