using Godot;
using System;

public partial class level1 : Node2D
{
	static int BLOCK_WIDTH = 54;
	static int BLOCK_HEIGHT = 10;
	private PackedScene _blockScene = (PackedScene)GD.Load("res://block.tscn");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player player = (Player)GetNode("Player");
		Vector2 PlayerPos = Vector2.Zero;
		PlayerPos.Y = (int)GetViewportRect().Size.Y - 100;
		player.Position = PlayerPos;

		MakeFloor();

		ScatterBlocks();
	}

	private void ScatterBlocks()
	{
		int TotalRows = (int)(int)GetViewportRect().Size.Y / BLOCK_HEIGHT;
		int TotalCols = (int)(int)GetViewportRect().Size.X / BLOCK_WIDTH;

		// GD.Print(TotalCols);
		// GD.Print(TotalRows);

		for (int r = 0; r < TotalRows; r++)
		{
			if (r % 10 == 0)
			{
				for (int c = 0; c < TotalCols; c++)
				{
					Random rnd = new Random();
					// more likely to have a block lower down
					int BaselineRowPercent = (int)(((float)r / (float)TotalRows) * 100);
					int PercentShouldMake = rnd.Next(BaselineRowPercent, 100);

					if (PercentShouldMake > 80)
					{
						int FiftyFifty = rnd.Next(0, 10);
						
						if (FiftyFifty >= 5)
						{
							Node2D block = (Node2D)_blockScene.Instantiate();
							block.Position = new Vector2(x: c * BLOCK_WIDTH, y: r * BLOCK_HEIGHT);
							GD.Print("Making a block at : " + block.Position);
							AddChild(block);
						}
					}
				}
			}
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
