using Godot;

public class FallState : PlayerState
{
	public FallState(Player player) : base(player) { }

    public override void Enter()
    {
        GD.Print("Entered Fall State");
    }

    public override void Update(double delta)
    {

        if(Input.IsActionJustPressed("jump")) {
        	player.ChangeState(player.jumpState);
        }

        var dir = Input.GetAxis("move_left","move_right");
        if(dir!=0) {
        	player.ChangeState(player.runState);
        }
        if(player.IsOnWall()) {
            player.ChangeState(player.wSlideState);
        }
    }

    public override void PhysicsUpdate(double delta)
    {
        Vector2 velocity=player.Velocity;
        var dir = Input.GetAxis("move_left","move_right");
        velocity.X=dir*player.MoveSpeed;
        velocity+=player.GetGravity()*(float)delta;
        player.Velocity=velocity;
    }
}