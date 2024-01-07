using Godot;
using System;

public partial class DemoText : Node
{
	RichTextLabel demoText;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		demoText = GetChild<RichTextLabel>(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void _on_timer_timeout()
	{
		// toggle visibility of "DEMO" text
		demoText.Visible = !demoText.Visible;
	}
}
