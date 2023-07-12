using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 PlayerPosition = Vector2.Zero;
		PlayerPosition.X = 100;
		PlayerPosition.Y = 400;


		// Vector2 PlayerPosition1 = GetNode("../Player").Position;
		// String BaseName = $"../Player".GetBaseName();

		Position = PlayerPosition;
	}
}
