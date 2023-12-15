using Godot;
using System;

public partial class RedBG : TextureRect
{
	bool fadeTriggered;
	float fadeTick = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Events.Instance.FinalKill += () => FadeBG();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (fadeTriggered){
			fadeTick -= 0.02f;
			this.SelfModulate = new Color(1, 1, 1, fadeTick);
		}
	}

	void FadeBG(){
		//GD.Print("fade bg");
		fadeTriggered = true;
	}
}
