using Godot;
using System;

public partial class PBLabel : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//this.Text = Math.Round(Oracle.Instance.displayTime, 2).ToString();
		if (Oracle.Instance.PBTime != 60){
			this.Text = Math.Round(Oracle.Instance.PBTime, 2).ToString();
		} else {
			this.Text = "";
		}

		if (Oracle.Instance.displayTime <= 0){
			this.Text = "";
		}
		
	}
}
