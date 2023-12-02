using Godot;
using System;

public partial class Database : Node
{
	public static Database Instance;

	public PackedScene bullet;
	public PackedScene transition;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;

		LoadStuffs();
	}

	void LoadStuffs(){
		bullet = GD.Load<PackedScene>("res://Prefabs/bullet.tscn");
		transition = GD.Load<PackedScene>("res://Prefabs/transition.tscn");
	}

}
