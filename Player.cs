using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const String NAME = "Player"; // just a place to put this
	public const String NAME_AREA2D = "player_area2D";
	private const float FULL_HEALTH = 5;
	private const float FULL_HEALTH_BAR = 30.0f;
	private float _health = 5;
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
	public override void _Draw()
	{
		DrawHealth();
	}

	private void DrawHealth()
	{
		float current_health_percentage = (_health / FULL_HEALTH);

		float current_health_bar = FULL_HEALTH_BAR * current_health_percentage;

		DrawRect(new Rect2(-17.0f, -20.0f, current_health_bar, 5.0f), Colors.Green);
	}

	public override void _PhysicsProcess(double delta)
	{
		Fire(delta);

		// _collisionInfo = MoveAndCollide(new Vector2(0, 0));

		HandleAnimation();

		HandleMovement(delta);

		QueueRedraw();
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


	private void _on_player_area_2d_area_shape_entered(Rid area_rid, Area2D area, long area_shape_index, long local_shape_index)
	{
		if (area.Name.Equals(enemy_wolf.NAME_AREA2D))
		{
			_health -= 1;

			if (_health == 0)
			{
				QueueFree();
			}
		}
	}
}


