using Godot;

public partial class Boss_2 : CharacterBody2D
{
    [Export] public Node2D TargetPlayer { get; set; }

    private NavigationAgent2D _navAgent;
    private const float Speed = 100f;

    public override void _Ready()
    {
        _navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        var timer = GetNode<Timer>("Timer");
        
        timer.Timeout += OnTimerTimeout;

        // Wait one physics frame so navigation is ready
        CallDeferred(nameof(InitializeNavigation));
    }

    private async void InitializeNavigation()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        MakePath();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_navAgent.IsNavigationFinished())
            return;

        Vector2 nextPos = _navAgent.GetNextPathPosition();
        Vector2 direction = GlobalPosition.DirectionTo(nextPos);

        Velocity = direction * Speed;
        MoveAndSlide();
    }

    private void MakePath()
    {
        if (TargetPlayer == null)
        {
            GD.Print("ERROR: TargetPlayer is not assigned!");
            return;
        }

        _navAgent.TargetPosition = TargetPlayer.GlobalPosition;
    }

    private void OnTimerTimeout()
    {
        MakePath();
    }
}