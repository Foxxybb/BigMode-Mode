using Godot;
using System;

public partial class GameScene : Node2D
{
	[Export] public AudioStreamWav musicForScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Events.Instance.EmitSignal("GameSceneReady");

		if (musicForScene != null){
			SoundManager.Instance.PlayMusic(musicForScene);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
