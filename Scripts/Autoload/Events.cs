using Godot;
using System;

public partial class Events : Node
{
	public static Events Instance;
	
	// CUSTOM SIGNALS MUST END WITH "EventHandler"

	// EXAMPLE:
	[Signal] public delegate void GameSceneReadyEventHandler(); // Emitted by game scene on scene load
	[Signal] public delegate void FinalKillEventHandler(); // Emitted by final enemy death

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}
}
