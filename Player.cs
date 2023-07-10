using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private AnimatedSprite2D _animatedSprite;
	int SCREEN_HORIZ = 1000;
	int SCREEN_VERT = 500;
	private const float Gravity = 12000.0f;
	int HORIZ_MOVEMENT = 100;
	int VERT_MOVEMENT = 1000;
	bool _isRight = true;

	private PackedScene _bulletScene = (PackedScene)GD.Load("res://bullet.tscn");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("player_animation");
	}

	public override void _PhysicsProcess(double delta)
	{
		// Move down 1 pixel per physics frame
		MoveAndCollide(new Vector2(0, 1));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// GD.Print("processing on player side");
		var velocity = Vector2.Zero; // The player's movement vector.

		if (Input.IsActionPressed("jump"))
		{
			velocity.Y -= VERT_MOVEMENT;
		}

		if (Input.IsActionPressed("right"))
		{
			velocity.X += HORIZ_MOVEMENT;
			_isRight = true;
			_animatedSprite.FlipH = true;
			_animatedSprite.Play("walking");

		}
		else if (Input.IsActionPressed("left"))
		{
			velocity.X -= HORIZ_MOVEMENT;
			_isRight = false;
			_animatedSprite.FlipH = false;
			_animatedSprite.Play("walking");
		}
		else
		{
			_animatedSprite.Stop();
		}

		Fire();

		ApplyMovement(velocity, delta);
	}

	private void Fire()
	{

		if (Input.IsActionPressed("fire"))
		{
			GD.Print("fire");
			bullet bullet = (bullet)_bulletScene.Instantiate();
			if (_isRight)
			{
				bullet.SetDirection(bullet.RIGHT);
			}
			if (!_isRight)
			{
				bullet.SetDirection(bullet.LEFT);
			}
			bullet.Transform = GlobalTransform;  
			Owner.AddChild(bullet);
		}
	}

	private void ApplyMovement(Vector2 velocity, double delta)
	{
		velocity.Y += (float)delta * Gravity;
		// translate new vector to this player's movement
		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, SCREEN_HORIZ),
			y: Mathf.Clamp(Position.Y, 0, SCREEN_VERT)
		);
	}
}
