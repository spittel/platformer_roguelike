using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private AnimatedSprite2D _animatedSprite;
	private const float GRAVITY = 10000.0f;
	int HORIZ_MOVEMENT = 100;
	int JUMP_MOVEMENT = 1000;
	bool _isRight = true;

	static double FIRE_WAIT = .1;
	double _lastFire = FIRE_WAIT + .0001;
	static int JUMP_NUM = 10;
	int _currentJump = 0;

	private PackedScene _bulletScene = (PackedScene)GD.Load("res://bullet.tscn");
	private KinematicCollision2D _collisionInfo;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("player_animation");
	}

	public override void _PhysicsProcess(double delta)
	{
		Fire(delta);
		_collisionInfo = MoveAndCollide(new Vector2(0, 0));

		var velocity = Vector2.Zero; // The player's movement vector.

		velocity = Jump(delta, velocity);

		if (Input.IsActionPressed("right"))
		{
			velocity.X += HORIZ_MOVEMENT;
			_isRight = true;
			_animatedSprite.FlipH = false;
			_animatedSprite.Play("walking");

		}
		else if (Input.IsActionPressed("left"))
		{
			velocity.X -= HORIZ_MOVEMENT;
			_isRight = false;
			_animatedSprite.FlipH = true;
			_animatedSprite.Play("walking");
		}
		else
		{
			_animatedSprite.Stop();
		}

		ApplyMovement(velocity, delta);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	private Vector2 Jump(double delta, Vector2 velocity)
	{
		if (Input.IsActionPressed("jump"))
		{
			if ( _currentJump < JUMP_NUM
				|| _collisionInfo != null // on ground, or colliding with something
				)
			{
				velocity.Y -= JUMP_MOVEMENT;
				
				_currentJump++;

				// player can hold jump down and jump a little higher, to the equivalent of 
				// JUMP_NUM
				if (_currentJump > JUMP_NUM)
					_currentJump = 0;
			}
		}

		return velocity;
	}

	private void Fire(double delta)
	{
		_lastFire += delta;

		// limit firing rate
		if (_lastFire < FIRE_WAIT)
		{
			return;
		}

		if (Input.IsActionPressed("fire"))
		{
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
			_lastFire = 0;
		}
	}

	private void ApplyMovement(Vector2 velocity, double delta)
	{
		velocity.Y += (float)delta * GRAVITY;
		// translate new vector to this player's movement
		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, (int)GetViewportRect().Size.X),
			y: Mathf.Clamp(Position.Y, 0, (int)GetViewportRect().Size.Y)
		);
	}
}
