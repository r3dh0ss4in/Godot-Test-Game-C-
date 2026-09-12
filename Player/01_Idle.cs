using Godot;

public class IdleState : PlayerState
{
	public IdleState(Player player) : base(player) { }

    public override void Enter()
    {
        GD.Print("Entered Idle State");
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
        if(!player.IsOnFloor()&&player.Velocity.Y>0) {
            player.ChangeState(player.fallState);
        }
    }
}