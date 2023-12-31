using Godot;
using System;

public partial class enemy_wolf : CharacterBody2D
{
	public static String NAME_AREA2D = "enemy_wolf_area2d";
	private const float FULL_HEALTH = 5;
	private const float FULL_HEALTH_BAR = 500.0f;
	private float _health = 5;
	private float _speed = 40.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public static float GRAVITY = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public static float JUMP_VELOCITY = GRAVITY * -4.0f;

	private AnimatedSprite2D _animatedSprite;

	RayCast2D _wolfRayCast;
	RayCast2D _bumpCast;
	RayCast2D _downRay1;
	RayCast2D _downRay2;

	private Boolean _huntMode = false;
	private bool _do_shake = false;
	private double _shake_time = 0;
	private bool _do_pace = true;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("enemy_wolf_animation");
		_wolfRayCast = GetNode<RayCast2D>("enemy_wolf_animation/WolfRayCast");
		_bumpCast = GetNode<RayCast2D>("enemy_wolf_animation/BumpRayCast");
		_downRay1 = GetNode<RayCast2D>("enemy_wolf_animation/DownRay1");
		_downRay2 = GetNode<RayCast2D>("enemy_wolf_animation/DownRay2");
	}

	public void SetDoPace(bool do_pace)
	{
		_do_pace = do_pace;
	}

	public override void _Draw()
	{
		DrawHealth();
	}

	private void DrawHealth()
	{
		float current_health_percentage = (_health / FULL_HEALTH);

		float current_health_bar = FULL_HEALTH_BAR * current_health_percentage;

		DrawRect(new Rect2(-250.0f, -500.0f, current_health_bar, 100.0f), Colors.Red);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		Vector2 direction = Vector2.Right;

		Shake(delta);

		CheckHuntMode();

		AdjustSpeed();

		Pace();

		direction = DecideDirection(direction);

		velocity = HandleInAir(delta, velocity);

		Velocity = CalculateVelocity(velocity, direction);

		MoveAndSlide();

		QueueRedraw();
	}

	private void Shake(double delta)
	{
		if (_do_shake)
		{
			_do_shake = false;
			_shake_time = 0.5;
		}

		if (_shake_time != 0)
		{
			Boolean ranBoolean = new Random().Next(0, 100) > 50;
			if (ranBoolean)
				Position = new Vector2(Position.X - 2, Position.Y);
			else
				Position = new Vector2(Position.X + 2, Position.Y);

			_shake_time -= delta;

			if (_shake_time < 0)
				_shake_time = 0;
		}
	}

	private Vector2 HandleInAir(double delta, Vector2 velocity)
	{
		if (!IsOnFloor())
		{
			velocity.Y += GRAVITY * (float)delta;
			if (_huntMode)
			{
				//jump towards target
				velocity.Y += JUMP_VELOCITY * (float)delta;
			}
		}

		return velocity;
	}

	private void Pace()
	{
		if (_do_pace)
		{
			if (!_downRay1.IsColliding() || !_downRay2.IsColliding() ||
			 // hitting world wall
			 _bumpCast.IsColliding())
			{
				if (!_huntMode)
				{
					_animatedSprite.FlipH = !_animatedSprite.FlipH;
					_wolfRayCast.TargetPosition = _wolfRayCast.TargetPosition * new Vector2(-1, 0);
					_bumpCast.TargetPosition = _bumpCast.TargetPosition * new Vector2(-1, 0);
				}
			}
		}
	}


	private Vector2 CalculateVelocity(Vector2 velocity, Vector2 direction)
	{
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * _speed;
			_animatedSprite.Play("walking");
		}
		else
		{
			_animatedSprite.Play("standing");
			velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
		}

		return velocity;
	}

	private void AdjustSpeed()
	{
		if (_huntMode)
			this._speed = 100.0f;
		else
			this._speed = 40.0f;
	}

	private Vector2 DecideDirection(Vector2 direction)
	{
		if (_do_pace)
		{
			if (!_animatedSprite.FlipH)
				direction = Vector2.Left;

			return direction;
		}
		else
		{
			return Vector2.Zero;
		}

	}

	private void CheckHuntMode()
	{
		if (_wolfRayCast.IsColliding())
		{
			GodotObject collided = _wolfRayCast.GetCollider();

			_huntMode = ((Godot.Node)collided).Name.Equals(Player.NAME);
		}
		else
		{
			_huntMode = false;
		}
	}
	private void _on_area_2d_area_shape_entered(Rid area_rid, Area2D area, long area_shape_index, long local_shape_index)
	{
		if (area.Name.Equals(bullet.NAME))
		{
			_health -= 1;
			_do_shake = true;

			if (_health == 0)
			{
				QueueFree();
			}
		}
	}


}




