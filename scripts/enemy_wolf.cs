using Godot;
using System;

public partial class enemy_wolf : CharacterBody2D
{
	private float _speed = 40.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public static float GRAVITY = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public static float JUMP_VELOCITY = GRAVITY * -4.0f;

	private AnimatedSprite2D _animatedSprite;

	RayCast2D _wolfRayCast;
	RayCast2D _downRay1;
	RayCast2D _downRay2;

	private Boolean _huntMode = false;

	public override void _Ready()
	{
		//$enemy_wolf_animation/WolfRayCast$enemy_wolf_animation/DownRay
		_animatedSprite = GetNode<AnimatedSprite2D>("enemy_wolf_animation");
		_wolfRayCast = GetNode<RayCast2D>("enemy_wolf_animation/WolfRayCast");
		_downRay1 = GetNode<RayCast2D>("enemy_wolf_animation/DownRay1");
		_downRay2 = GetNode<RayCast2D>("enemy_wolf_animation/DownRay2");
	}

	public override void _Draw()
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		Vector2 direction = Vector2.Right;

		CheckHuntMode();

		AdjustSpeed();

		Pace();

		direction = DecideDirection(direction);

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += GRAVITY * (float)delta;
			if (_huntMode)
			{
				//jump towards target
				velocity.Y += JUMP_VELOCITY * (float)delta;
			}
		}



		Velocity = CalculateVelocity(velocity, direction);

		MoveAndSlide();
	}

	private void Pace()
	{
		if (!_downRay1.IsColliding() || !_downRay2.IsColliding())
		{
			if (!_huntMode)
			{
				_animatedSprite.FlipH = !_animatedSprite.FlipH;
				_wolfRayCast.TargetPosition = _wolfRayCast.TargetPosition * new Vector2(-1, 0);
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
		{
			this._speed = 100.0f;
		}
		else
		{
			this._speed = 40.0f;
		}
	}

	private Vector2 DecideDirection(Vector2 direction)
	{
		if (!_animatedSprite.FlipH)
			direction = Vector2.Left;

		return direction;
	}

	private void CheckHuntMode()
	{
		if (_wolfRayCast.IsColliding())
		{
			GodotObject collided = _wolfRayCast.GetCollider();

			if (((Godot.Node)collided).Name.Equals(Player.NAME))
				_huntMode = true;
		}
		else
		{
			_huntMode = false;
		}
	}
}
