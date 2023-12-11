using Godot;
using System;

public partial class Oracle : Node
{
	public static Oracle Instance;

	public Node CurrentScene { get; set; }
	string nextScenePath; // used to store path to delay scene change until transition animation finishes

	AnimationPlayer transition;
	MyCamera cam;

	public bool myDebug = true;

	public double displayTime;
	public double PBTime = 60;
	public bool displayTimerOn;
	public bool playerDead;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;

		Events.Instance.GameSceneReady += () => OnNewScene();

		Viewport root = GetTree().Root;
		CurrentScene = root.GetChild(root.GetChildCount() - 1);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (displayTimerOn){
			displayTime -= delta;
		}

		if (Input.IsActionJustPressed("reset_action"))
		{
			//ResetScene();
			// if current scene is not gameplay scene, change scene to Tutorial
			// else, change scene to main game scene
			if (GetNode<GameScene>("/root/Scene").isGameplayScene){
				ChangeScene("res://Scenes/testScene.tscn");
			} else {
				ChangeScene("res://Scenes/training.tscn");
			};

			SoundManager.Instance.StopMusic();
		}

		if (Input.IsActionJustPressed("test_action"))
		{
			if (DisplayServer.WindowGetMode() == (DisplayServer.WindowMode.ExclusiveFullscreen)){
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			} else {
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
			}
		}

		//if (Input.IsActionJustPressed("test_action"))
		//{
		//	GD.Print("test");
		//}

		//if (Input.IsActionJustPressed("debug_action"))
		//{
		//	myDebug = !myDebug;
		//	GD.Print("debug: " + myDebug);
		//}
	}

	void OnNewScene(){
		cam = CurrentScene.GetNode<MyCamera>("MyCamera");

		transition = GetNode<AnimationPlayer>("/root/Scene/MyCamera/Transition/TransitionAnimator");
		transition.AnimationFinished += _on_transition_end;

		displayTimerOn = true;
		displayTime = 60;
		playerDead = false;
	}

	public void ResetScene(){
		GD.Print("Reset Scene");
		// play transition out
		transition.Play("transition_out");
		
		// reset scene
		nextScenePath = GetTree().CurrentScene.SceneFilePath;
	}

	public void ChangeScene(string path){
		GD.Print("Change Scene");
		// play transition out
		transition.Play("transition_out");

		nextScenePath = path;
	}

	// Scene change script
	public void GoToScene(string path)
	{
		// call is deferred to prevent crashing when changing scenes
		CallDeferred(nameof(DeferredGotoScene), path);

		// play transition_in animation
		transition.Play("transition_in");
	}

	public void DeferredGotoScene(string path)
	{
		// It is now safe to remove the current scene
		CurrentScene.Free();

		// Load a new scene.
		var nextScene = (PackedScene)GD.Load(path);

		// Instance the new scene.
		CurrentScene = nextScene.Instantiate();

		// Add it to the active scene, as child of root.
		GetTree().Root.AddChild(CurrentScene);

		// Optionally, to make it compatible with the SceneTree.change_scene_to_file() API.
		GetTree().CurrentScene = CurrentScene;
	}

	void _on_transition_end(StringName anim_name)
	{
		// if transitioning OUT, trigger scene change
		if (anim_name == "transition_out"){
			GoToScene(nextScenePath);
		}
	}
}
