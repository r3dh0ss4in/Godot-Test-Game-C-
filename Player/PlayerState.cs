using Godot;

public abstract class PlayerState
{
    protected Player player;

    public PlayerState(Player player)
    {
        this.player=player;
    }

    public virtual void Enter() {}
    public virtual void Update(double delta) {}
    public virtual void PhysicsUpdate(double delta) {}
    public virtual void Exit() {}
}