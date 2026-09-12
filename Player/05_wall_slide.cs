using Godot;

public class WSlideState : PlayerState
{
	public WSlideState(Player player) : base(player) { }

    public override void Enter()
    {
        GD.Print("Entered Wall Slide State");

        Vector2 velocity=player.Velocity;
        velocity.X=0;
        player.Velocity=velocity;
    }

    public override void Update(double delta)
    {
        var dir = Input.GetAxis("move_left","move_right");
        
        if(Input.IsActionJustPressed("jump")) {
            player.ChangeState(player.wJumpState);
        }

        if(dir!=0&&!player.IsOnFloor()) {
            player.ChangeState(player.fallState);
        } else if(dir!=0&&player.IsOnFloor()) {
            player.ChangeState(player.runState);
        } else if(dir==0&&player.IsOnFloor()) {
            player.ChangeState(player.idleState);
        }
    }

    public override void PhysicsUpdate(double delta)
    {
        Vector2 velocity = player.Velocity;
        velocity.Y=player.WallSlideSpeed;
        player.Velocity=velocity;
    }
}