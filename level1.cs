using Godot;
using System;

public partial class level1 : Node2D
{
	private PackedScene _blockScene = (PackedScene)GD.Load("res://block.tscn");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 0; i < 4; i++)
		{
			Random rnd = new Random();
			int RanX = rnd.Next(0, 1000);
			int RanY = rnd.Next(0, 800);
			Node2D block = (Node2D)_blockScene.Instantiate();
			block.Position = new Vector2(x: RanX, y: RanY);
			AddChild(block);
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
