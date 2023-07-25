using Godot;
using System;

public partial class Player : CharacterBody2D
{

	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	private AnimatedSprite2D _animatedSprite;
	bool _isRight = true;

	static double FIRE_WAIT = .1;
	double _lastFire = FIRE_WAIT + .0001;

	private PackedScene _bulletScene = (PackedScene)GD.Load("res://bullet.tscn");
	private KinematicCollision2D _collisionInfo;


	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("player_animation");
	}

	public override void _PhysicsProcess(double delta)
	{
		Fire(delta);

		// _collisionInfo = MoveAndCollide(new Vector2(0, 0));

		HandleAnimation();

		HandleMovement(delta);
	}

	private void HandleMovement(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("left", "right", "jump", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void HandleAnimation()
	{
		if (Input.IsActionPressed("right"))
		{
			_isRight = true;
			_animatedSprite.FlipH = false;
			_animatedSprite.Play("walking");

		}
		else if (Input.IsActionPressed("left"))
		{
			_isRight = false;
			_animatedSprite.FlipH = true;
			_animatedSprite.Play("walking");
		}
		else
		{
			_animatedSprite.Stop();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

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


}
