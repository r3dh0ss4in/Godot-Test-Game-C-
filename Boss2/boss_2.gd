extends CharacterBody2D

@onready var nav_agent: NavigationAgent2D = $NavigationAgent2D
@export var target_player: Node2D

const SPEED = 120.0

func _ready() -> void:
	# Wait one frame so navigation is ready
	await get_tree().physics_frame
	make_path()

func _physics_process(delta: float) -> void:
	if nav_agent.is_navigation_finished():
		return

	var next_pos = nav_agent.get_next_path_position()
	var direction = global_position.direction_to(next_pos)
	
	velocity = direction * SPEED
	move_and_slide()

func make_path() -> void:
	if target_player == null:
		print("ERROR: target_player is not assigned!")
		return
	
	nav_agent.target_position = target_player.global_position
	print("Path set to: ", target_player.global_position)

func _on_timer_timeout() -> void:
	make_path()
