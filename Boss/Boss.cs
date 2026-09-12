using Godot;
using System.Collections.Generic;

public partial class Boss : CharacterBody2D
{
    public Node2D TargetPlayer { get; private set; }
    public bool PlayerInAttackRange { get; private set; }
    public BossStateMachine StateMachine { get; private set; }

    public override void _Ready()
    {
        var chaseArea = GetNode<Area2D>("ChaseArea");
        var attackArea = GetNode<Area2D>("AttackArea");

        chaseArea.BodyEntered += OnChaseAreaEntered;
        chaseArea.BodyExited += OnChaseAreaExited;
        attackArea.BodyEntered += OnAttackAreaEntered;
        attackArea.BodyExited += OnAttackAreaExited;

        StateMachine = new BossStateMachine();
        StateMachine.Initialize(this); // this = boss node
    }

    public override void _PhysicsProcess(double delta)
    {
        StateMachine.PhysicsUpdate(delta);
    }

    public override void _Process(double delta)
    {
        StateMachine.Update(delta);
    }

    private static bool IsPlayer(Node2D body)
    {
        return body.IsInGroup("Player") || body is Player;
    }

    private void OnChaseAreaEntered(Node2D body)
    {
        if (IsPlayer(body))
        {
            TargetPlayer = body;
        }
    }

    private void OnChaseAreaExited(Node2D body)
    {
        if (body == TargetPlayer)
        {
            TargetPlayer = null;
        }
    }

    private void OnAttackAreaEntered(Node2D body)
    {
        if (IsPlayer(body))
        {
            TargetPlayer = body;
            PlayerInAttackRange = true;
        }
    }

    private void OnAttackAreaExited(Node2D body)
    {
        if (IsPlayer(body))
        {
            PlayerInAttackRange = false;
        }
    }
}

public abstract class BossState
{
    protected Boss Character;
    protected BossStateMachine StateMachine;

    public virtual void Init(Boss character, BossStateMachine stateMachine)
    {
        Character = character;
        StateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update(double delta) { }
    public virtual void PhysicsUpdate(double delta) { }
}

public class BossStateMachine
{
    public BossState CurrentState { get; private set; }
    private readonly Dictionary<string, BossState> _states = new();

    public void Initialize(Boss boss)
    {
        RegisterState("patrol", new BossPatrolState(), boss);
        RegisterState("chase", new BossChaseState(), boss);
        RegisterState("attack", new BossAttackState(), boss);
        TransitionTo("patrol");
    }

    private void RegisterState(string name, BossState state, Boss boss)
    {
        state.Init(boss, this);
        _states[name] = state;
    }

    public void TransitionTo(string newStateName)
    {
        if (!_states.TryGetValue(newStateName, out var nextState))
        {
            GD.PrintErr($"State '{newStateName}' does not exist in BossStateMachine!");
            return;
        }

        CurrentState?.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void PhysicsUpdate(double delta) => CurrentState?.PhysicsUpdate(delta);
    public void Update(double delta) => CurrentState?.Update(delta);
}

public class BossPatrolState : BossState
{
    public float MoveSpeed = 100f;
    private float _direction = 1.0f;

    public override void PhysicsUpdate(double delta)
    {
        if (Character.PlayerInAttackRange)
        {
            StateMachine.TransitionTo("attack");
            return;
        }

        if (Character.TargetPlayer != null)
        {
            StateMachine.TransitionTo("chase");
            return;
        }

        Vector2 velocity = Character.Velocity;
        if (!Character.IsOnFloor())
        {
            velocity += Character.GetGravity() * (float)delta;
        }
        else
        {
            velocity.X = _direction * MoveSpeed;
        }

        Character.Velocity = velocity;
        Character.MoveAndSlide();

        if (Character.IsOnWall())
        {
            _direction *= -1.0f;
        }
    }
}

public class BossChaseState : BossState
{
    public float ChaseSpeed = 200f;

    public override void Enter()
    {
        GD.Print("Boss: Chasing");
    }

    public override void PhysicsUpdate(double delta)
    {
        if (Character.PlayerInAttackRange)
        {
            StateMachine.TransitionTo("attack");
            return;
        }

        var target = Character.TargetPlayer;
        if (target == null)
        {
            StateMachine.TransitionTo("patrol");
            return;
        }

        Vector2 velocity = Character.Velocity;
        if (!Character.IsOnFloor())
        {
            velocity += Character.GetGravity() * (float)delta;
        }
        else
        {
            float dirToPlayer = Mathf.Sign(target.GlobalPosition.X - Character.GlobalPosition.X);
            if (dirToPlayer != 0)
            {
                velocity.X = dirToPlayer * ChaseSpeed;
            }
        }

        Character.Velocity = velocity;
        Character.MoveAndSlide();
    }
}

public class BossAttackState : BossState
{
    public override void Enter()
    {
        GD.Print("Boss: Attacking");
    }

    public override void PhysicsUpdate(double delta)
    {
        if (!Character.PlayerInAttackRange)
        {
            StateMachine.TransitionTo(Character.TargetPlayer != null ? "chase" : "patrol");
            return;
        }

        Vector2 velocity = Character.Velocity;
        if (!Character.IsOnFloor())
        {
            velocity += Character.GetGravity() * (float)delta;
        }
        else
        {
            velocity.X = 0;
        }

        Character.Velocity = velocity;
        Character.MoveAndSlide();
    }
}