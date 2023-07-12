using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
	int MID_WAY = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MID_WAY = (int)GetViewportRect().Size.X / 2;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 CameraPosition = Vector2.Zero;

		Player p = (Player)GetNode("../Player");

		CameraPosition.Y = p.Position.Y;
		CameraPosition.X = MID_WAY;

		Position = CameraPosition;
	}
}
