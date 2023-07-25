using Godot;
using System;

public partial class enemy_wolf : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimatedSprite2D _animatedSprite;
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("enemy_wolf_animation");
	}

	public override void _Draw()
	{
	   
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		var spaceState = GetWorld2D().DirectSpaceState;

		var query = PhysicsRayQueryParameters2D.Create(Vector2.Zero, new Vector2(1200, 0));

		query.Exclude = new Godot.Collections.Array<Rid>{GetRid()};
		var result = spaceState.IntersectRay(query);

		if (result.Count > 0)
		{
			GD.Print("wolf found something!");
			GD.Print(result);
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
