using Godot;
using System;

public partial class Main : Node2D
{
	public override void _Ready()
	{
		// Signal connection
		var area = GetNode<Area2D>("Area2D");
    	area.BodyEntered += OnBodyEntered;
	}

	public void OnBodyEntered(Node2D body)
	{
		if(body is Player) {
			GD.Print("Game Over");
			// Used CallDeferred instal directly using 
			// ReloadCurrentScene so it will finish physics process
			GetTree().CallDeferred(SceneTree.MethodName.ReloadCurrentScene);
		}
	}
}
