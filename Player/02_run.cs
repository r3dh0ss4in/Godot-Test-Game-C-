using Godot;

public class RunState : PlayerState
{
	public RunState(Player player) : base(player) { }

    public override void Enter()
    {
        GD.Print("Entered Run State");
    }

    public override void Update(double delta)
    {
        if(Input.IsActionJustPressed("jump")) {
            player.ChangeState(player.jumpState);
        }
        var dir = Input.GetAxis("move_left","move_right");
        
        if(dir==0) {
        	player.ChangeState(player.idleState);
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