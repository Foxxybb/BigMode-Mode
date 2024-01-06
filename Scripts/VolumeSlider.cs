using Godot;
using System;

public partial class VolumeSlider : Node
{

	[Export] string busName;

	int masterBusIndex;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// signal method
	private void _on_value_changed(double value)
	{
		AudioServer.SetBusVolumeDb(0, (float)value);
	}

}


