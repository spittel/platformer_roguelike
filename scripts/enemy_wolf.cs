using Godot;
using System;

public partial class enemy_wolf : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimatedSprite2D _animatedSprite;

	RayCast2D wolfRayCast;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("enemy_wolf_animation");
		wolfRayCast = GetNode<RayCast2D>("WolfRayCast");
	}

	public override void _Draw()
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (wolfRayCast.IsColliding())
		{
			GD.Print("wolf found something!");
			GodotObject collided = wolfRayCast.GetCollider();
			GD.Print(((Godot.Node)collided).Name);
		}

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// // Handle Jump.
		// if (Input.IsActionJustPressed("jump") && IsOnFloor())
		// 	velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

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
