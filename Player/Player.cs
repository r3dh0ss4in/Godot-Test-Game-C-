using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private PlayerState currentState;

	[Export] public float MoveSpeed=300f;
	[Export] public float JumpVelocity=-400f;
	[Export] public float WallSlideSpeed=100f;
	[Export] public Vector2 WallJumpForce = new Vector2(300.0f, -380.0f);

	public PlayerState idleState;
	public PlayerState runState;
	public PlayerState jumpState;
	public PlayerState fallState;
	public PlayerState wSlideState;
	public PlayerState wJumpState;

	public override void _Ready() {
		idleState = new IdleState(this);
		runState = new RunState(this);
		jumpState = new JumpState(this);
		fallState = new FallState(this);
		wSlideState = new WSlideState(this);
		wJumpState = new WJumpState(this);

		ChangeState(idleState);
	}

    public override void _Process(double delta)
    {
        currentState?.Update(delta);
    }
    public override void _PhysicsProcess(double delta)
    {
    	Vector2 velocity=Velocity;
    	if(!IsOnFloor()) {
    		velocity+=GetGravity()*(float)delta;
    	}
    	Velocity=velocity;
    	currentState?.PhysicsUpdate(delta);
    	MoveAndSlide();
    }
    public void ChangeState(PlayerState newState) {
    	currentState?.Exit();
    	currentState=newState;
    	currentState?.Enter();
    }
}
