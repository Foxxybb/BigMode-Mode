using Godot;
using System;

public partial class Database : Node
{
	public static Database Instance;

	//prefabs
	public PackedScene bullet;
	public PackedScene enemy;
	public PackedScene enemyG;
	public PackedScene enemyA;
	public PackedScene transition;
	public PackedScene audioShot;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;

		LoadStuffs();
	}

	void LoadStuffs(){
		bullet = GD.Load<PackedScene>("res://Prefabs/bullet.tscn");
		enemy = GD.Load<PackedScene>("res://Prefabs/enemy.tscn");
		enemyG = GD.Load<PackedScene>("res://Prefabs/enemyG.tscn");
		enemyA = GD.Load<PackedScene>("res://Prefabs/enemyA.tscn");
		transition = GD.Load<PackedScene>("res://Prefabs/transition.tscn");
		audioShot = GD.Load<PackedScene>("res://Prefabs/AudioShot.tscn");
	}

}
