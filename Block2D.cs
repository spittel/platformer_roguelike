using Godot;
using System;

public partial class Block2D : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_body_entered(Node2D body)
	{ 
		GD.Print("block hit, but by what");
		if (body.IsInGroup("bullets"))
		{
			GD.Print("hit by bullet");

		}
	}
}



