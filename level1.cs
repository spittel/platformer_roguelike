using Godot;
using System;

public partial class level1 : Node2D
{
	static int BLOCK_WIDTH = 54;
	static int BLOCK_HEIGHT = 7;
	private PackedScene _blockScene = (PackedScene)GD.Load("res://block.tscn");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player player = (Player)GetNode("Player");
		Vector2 PlayerPos = Vector2.Zero;
		PlayerPos.Y = (int)GetViewportRect().Size.Y -100;
		player.Position = PlayerPos;
		
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


		for (int i = 0; i < 30; i++)
		{
			Node2D block = (Node2D)_blockScene.Instantiate();
			block.Position = new Vector2(x: 0, y: i * (BLOCK_HEIGHT + 30));
			AddChild(block);
		}
	}

	private void MakeFloor()
	{
		int FloorLength = 0;
		int i = 0;

		while (FloorLength < (int)GetViewportRect().Size.X)
		{
			FloorLength = i * BLOCK_WIDTH;

			Node2D block = (Node2D)_blockScene.Instantiate();
			block.Position = new Vector2(x: FloorLength, y: (int)GetViewportRect().Size.Y);
			AddChild(block);
			
			i++;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
