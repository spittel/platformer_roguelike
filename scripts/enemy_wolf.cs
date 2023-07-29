using Godot;
using System;

public partial class enemy_wolf : CharacterBody2D
{
	public const float Speed = 100.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimatedSprite2D _animatedSprite;

	RayCast2D _wolfRayCast;
	RayCast2D _downRay1;
	RayCast2D _downRay2;

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

		if (_wolfRayCast.IsColliding())
		{
			GD.Print("wolf found something!");
			GodotObject collided = _wolfRayCast.GetCollider();
			GD.Print(((Godot.Node)collided).Name);
			GD.Print(((Godot.Node)collided).Name.Equals(Player.NAME));
		}

		// have the wolf pace
		if (!_downRay1.IsColliding() || !_downRay2.IsColliding())
		{
			_animatedSprite.FlipH = !_animatedSprite.FlipH;
			_wolfRayCast.TargetPosition =_wolfRayCast.TargetPosition * new Vector2(-1, 0);
		}

		Vector2 direction = Vector2.Right;
		if (!_animatedSprite.FlipH)
			direction = Vector2.Left;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;



		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			_animatedSprite.Play("walking");
		}
		else
		{
			_animatedSprite.Play("standing");
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;

		MoveAndSlide();
	}
}
