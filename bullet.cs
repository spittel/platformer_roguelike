using Godot;
using System;

public partial class bullet : Node2D
{
	// area2D name, we just store it here so we have a mental connection to it
	public static String NAME = "bullet_area_2d";

	int MAX_X_DISTANCE = 1200;
	int MAX_Y_DISTANCE = 1000;
	public static String RIGHT = "Right";
	public static String LEFT = "Left";
	String _dir = RIGHT;
	private AnimatedSprite2D _animatedBulletSprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedBulletSprite = GetNode<AnimatedSprite2D>("animated_bullet");
		_animatedBulletSprite.Play("fire");
	}


	public void SetDirection(String dir)
	{
		_dir = dir;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;

		if (_dir == RIGHT)
		{
			velocity.X = 1000;
			_animatedBulletSprite.FlipH = false;
		}
		else if (_dir == LEFT)
		{
			velocity.X = -1000;
			_animatedBulletSprite.FlipH = true;
		}
		Position += velocity * (float)delta;

		if (Math.Abs(Position.X) > MAX_X_DISTANCE || Math.Abs(Position.Y) > MAX_Y_DISTANCE)
		{
			QueueFree();
		}
	}

	private void _on_area_2d_area_shape_entered(Rid area_rid, Area2D area, long area_shape_index, long local_shape_index)
	{
		// remove bullet when it hits something
		if (!area.Name.Equals(Player.NAME_AREA2D))
		{
			QueueFree();
		}
	}
	private void _on_area_2d_body_entered(Node2D body)
	{
		if (!body.Name.Equals(Player.NAME))
		{
			QueueFree();
		}
	}

}








