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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("test_action")){
			GD.Print("spawn");
			SpawnEnemy(0, 30);
			SpawnEnemy(1, 20);
			SpawnEnemy(2, 10);
			SpawnEnemy(3, 30);
			SpawnEnemy(4, 20);
			SpawnEnemy(5, 10);
		}
	}

	void SpawnEnemy(int spawnIdx, int HP){
		// spawn enemy
		Enemy newEnemy = (Enemy)Database.Instance.enemy.Instantiate();
		GetNode<Node2D>("/root/Scene").AddChild(newEnemy);

		// set enemy position
		newEnemy.GlobalPosition = spawnList[spawnIdx].Position;

		// set enemy movement vector
		if (spawnIdx > 2){
			newEnemy.movementVec = new Vector2(60,0);
		} else {
			newEnemy.movementVec = new Vector2(-60,0);
		}
		
		// set enemy HP
		newEnemy.HP = HP;

	}

	
}
