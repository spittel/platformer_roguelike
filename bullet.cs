using Godot;
using System;

public partial class bullet : Node2D
{
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
	}



}
