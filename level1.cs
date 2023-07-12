using Godot;
using System;

public partial class level1 : Node2D
{
	static int BLOCK_WIDTH = 54;
	private PackedScene _blockScene = (PackedScene)GD.Load("res://block.tscn");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MakeFloor();

		ScatterBlocks();
	}

	private void ScatterBlocks()
	{
		for (int i = 0; i < 20; i++)
		{
			// create a set of rows with a player's height between them
			// randomly 
			Random rnd = new Random();
			int RanX = rnd.Next(0, (int)GetViewportRect().Size.X);
			int RanY = rnd.Next(0, (int)GetViewportRect().Size.Y - 64);
			Node2D block = (Node2D)_blockScene.Instantiate();
			block.Position = new Vector2(x: RanX, y: RanY);
			AddChild(block);
		}
	}

	private void MakeFloor()
	{
		for (int i = 0; i < 25; i++)
		{
			Node2D block = (Node2D)_blockScene.Instantiate();
			block.Position = new Vector2(x: i * BLOCK_WIDTH, y: (int)GetViewportRect().Size.Y);
			AddChild(block);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
