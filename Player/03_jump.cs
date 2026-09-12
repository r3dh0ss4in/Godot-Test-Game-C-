using Godot;

public class JumpState : PlayerState
{
	public JumpState(Player player) : base(player) { }

    public override void Enter()
    {
        GD.Print("Entered Jump State");
        if(player.IsOnFloor()) {
            Vector2 velocity=player.Velocity;
            velocity.Y=player.JumpVelocity;
            player.Velocity=velocity;
        }
    }

    public override void Update(double delta)
    {
        var dir = Input.GetAxis("move_left","move_right");
        
        if(player.IsOnFloor()&&dir==0) {
            player.ChangeState(player.idleState);
        } else if(player.IsOnFloor()&&dir!=0) {
            player.ChangeState(player.runState);
        }

        if(player.IsOnWall()) {
            player.ChangeState(player.wSlideState);
        }

        if(!player.IsOnFloor()&&player.Velocity.Y>0) {
            player.ChangeState(player.fallState);
        }
    }
    
    public override void PhysicsUpdate(double delta)
    {
        Vector2 velocity=player.Velocity;
        var dir = Input.GetAxis("move_left","move_right");
        velocity.X=dir*player.MoveSpeed;
        player.Velocity=velocity;
    }
}