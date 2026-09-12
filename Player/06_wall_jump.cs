using Godot;

public class WJumpState : PlayerState
{
    private float _jumpTimer = 0.0f;
    private const float JumpDuration = 0.2f;

	public WJumpState(Player player) : base(player) { }

    public override void Enter()
    {
        GD.Print("Entered Wall Jump State");
        Vector2 wallNormal = player.GetWallNormal();
        Vector2 velocity = player.Velocity;

        velocity.X = wallNormal.X * player.WallJumpForce.X;
        velocity.Y = player.WallJumpForce.Y;

        player.Velocity = velocity;
        _jumpTimer = JumpDuration;
    }

    public override void Update(double delta)
    {
        if(player.Velocity.Y>0&&!player.IsOnFloor()) {
            player.ChangeState(player.fallState);
        }
        if(player.IsOnWall()) {
            player.ChangeState(player.wSlideState);
        }
    }
    
    public override void PhysicsUpdate(double delta)
    {
        Vector2 velocity = player.Velocity;

        if (_jumpTimer <= 0)
        {
            var dir = Input.GetAxis("move_left", "move_right");
            velocity.X = Mathf.Lerp(velocity.X, dir * player.MoveSpeed, 0.1f);
        }

        player.Velocity = velocity;
    }
}