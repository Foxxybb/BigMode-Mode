using Godot;
using System;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//GetNode<VBoxContainer>("VBoxContainer").GetNode<Button>("Play").GrabFocus();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_play_pressed()
	{
		Oracle.Instance.ChangeScene("res://Scenes/training.tscn");

		var SMI = SoundManager.Instance;
		if (!SMI.musicPlayer.Playing){
			SMI.PlayMusic(SMI.introMusic);
		}
	}

	private void _on_about_pressed()
	{
		Oracle.Instance.ChangeScene("res://Scenes/about.tscn");
		
		var SMI = SoundManager.Instance;
		if (!SMI.musicPlayer.Playing){
			SMI.PlayMusic(SMI.introMusic);
		}
	}

	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}

	private void _on_demo_pressed()
	{
		SoundManager.Instance.StopMusic();
		Oracle.Instance.ChangeScene("res://Scenes/demoScene.tscn");
	}
}
