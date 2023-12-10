using Godot;
using System;

public partial class MovingBG : Node2D
{
	Node2D block1;
	Node2D block2;
	Node2D block3;
	Node2D block4;
	Node2D block5;

	bool move1;
	bool move2;
	bool move3;
	bool move4;
	bool move5;

	int eventTick = 0;
	float blockSpeed = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// get parts of background
		block1 = this.GetChild<Node2D>(0);
		block2 = this.GetChild<Node2D>(1);
		block3 = this.GetChild<Node2D>(2);
		block4 = this.GetChild<Node2D>(3);
		block5 = this.GetChild<Node2D>(4);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!Oracle.Instance.playerDead){
			if (move1){
				block1.Position = new Vector2(block1.Position.X, block1.Position.Y + blockSpeed);
			}
			if (move2){
				block2.Position = new Vector2(block2.Position.X, block2.Position.Y - blockSpeed);
			}
			if (move3){
				block3.Position = new Vector2(block3.Position.X, block3.Position.Y + blockSpeed);
			}
			if (move4){
				block4.Position = new Vector2(block4.Position.X, block4.Position.Y - blockSpeed);
			}
			if (move5){
				block5.Position = new Vector2(block5.Position.X, block5.Position.Y + blockSpeed);
			}
		}
		
	}

	private void _on_event_timer_timeout()
	{
		if (!Oracle.Instance.playerDead)
		{
			// trigger next event in sequence
			eventTick++;
			MoveEvent(eventTick);
		}
	}

	void MoveEvent(int tick)
	{
		switch (tick)
		{
			case 10:
				move1 = true;
				break;
			case 20:
				move5 = true;
				break;
			case 30:
				move2 = true;
				break;
			case 40:
				move4 = true;
				break;
			case 50:
				move3 = true;
				break;
			case 60:
				break;
			default:
				break;
		}
	}
}
