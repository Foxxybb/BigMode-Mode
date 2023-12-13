using Godot;
using System;

public partial class Database : Node
{
	public static Database Instance;

	// game scenes
	public PackedScene tutScene;
	public PackedScene mainGameScene;

	// prefabs
	public PackedScene bullet;
	public PackedScene enemy;
	public PackedScene enemyG;
	public PackedScene enemyA;
	public PackedScene enemyB;
	public PackedScene enemyF;
	public PackedScene transition;
	public PackedScene audioShot;
	public PackedScene warn;

	public PackedScene deathParticles;
	public PackedScene killParticles;
	public PackedScene killParticlesB;
	public PackedScene finalParticles;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;

		//LoadScenes();

		LoadStuffs();
	}

	void LoadStuffs(){
		bullet = GD.Load<PackedScene>("res://Prefabs/bullet.tscn");
		enemy = GD.Load<PackedScene>("res://Prefabs/enemy.tscn");
		enemyG = GD.Load<PackedScene>("res://Prefabs/enemyG.tscn");
		enemyA = GD.Load<PackedScene>("res://Prefabs/enemyA.tscn");
		enemyB = GD.Load<PackedScene>("res://Prefabs/enemyB.tscn");
		enemyF = GD.Load<PackedScene>("res://Prefabs/enemyF.tscn");
		transition = GD.Load<PackedScene>("res://Prefabs/transition.tscn");
		audioShot = GD.Load<PackedScene>("res://Prefabs/AudioShot.tscn");
		warn = GD.Load<PackedScene>("res://Prefabs/warn.tscn");

		deathParticles = GD.Load<PackedScene>("res://Prefabs/deathParticles.tscn");
		killParticles = GD.Load<PackedScene>("res://Prefabs/killParticles.tscn");
		killParticlesB = GD.Load<PackedScene>("res://Prefabs/killParticlesB.tscn");
		finalParticles = GD.Load<PackedScene>("res://Prefabs/finalParticles.tscn");
	}

	void LoadScenes(){
		tutScene = GD.Load<PackedScene>("res://Scenes/training.tscn");
		mainGameScene = GD.Load<PackedScene>("res://Scenes/testScene.tscn");
	}

}
