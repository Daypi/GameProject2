using System;
using UnityEngine;

public class MovementGestion {
	
	PlayerInfo playerinfo;
	CharacterController character;
	// The speed when walking
	float walkSpeed = 2.0f;
	// after trotAfterSeconds of walking we trot with trotSpeed
	public float trotSpeed = 8.0f;
	// when pressing "Fire3" button (cmd) we start running
	float runSpeed = 6.0f;
	
	float inAirControlAcceleration = 3.0f;
	
	// How high do we jump when pressing jump and letting go immediately
	float jumpHeight = 0.1f;
	
	// The gravity for the character
	float gravity = 1.0f;
	float gravityWall = 0.1f;
	bool didApplyGravity = true;
	// The gravity in controlled descent mode
	float speedSmoothing = 10.0f;
	float rotateSpeed = 500.0f;
	float trotAfterSeconds = 3.0f;
	float WallJumpDuration = 0.5F;
	float WallClimbUpDuration = 0.3f;
	float WallClimbSideDuratin = 0.1f;
	float JumpLatence = 0.0f;
	float JumpLatenceCurrentTime = -1.0f;

	bool canJump = true;
	public bool pushjump = false;
	
	private float jumpRepeatTime = 0.00f;
	private float jumpTimeout = 0.15f;
	private float groundedTimeout = 0.25f;
	
	// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
	private float lockCameraTimer = 0.0f;
	
	// The current move direction in x-z
	private Vector3 moveDirection = Vector3.zero;
	// The current vertical speed
	private float verticalSpeed = 0.0f;
	// The current x-z move speed
	private float moveSpeed = 0.0f;
	
	// The last collision flags returned from controller.Move
	private CollisionFlags collisionFlags; 
	
	// Are we jumping? (Initiated with jump button and not grounded yet)
	private bool jumping = false;
	private bool jumpingReachedApex = false;
	
	// Are we moving backwards (This locks the camera to not do a 180 degree spin)
	private bool movingBack = false;
	// Is the user pressing any keys?
	private bool isMoving = false;
	// When did the user start walking (Used for going into trot after a while)
	private float walkTimeStart = 0.0f;
	// Last time the jump button was clicked down
	private float lastJumpButtonTime = -10.0f;
	// Last time we performed a jump
	private float lastJumpTime = -1.0f;
	private float WallJumpCurrentTime = -1.0f;
	private int WallJumpDirection = 0;
	private float WallClimbCurrentTime = -1.0F;
	// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
	private float lastJumpStartHeight = 0.0f;
	
	private float lastGroundedTime = 0.0f;
	
	
	private bool isControllable = true;
	
	public MovementGestion(CharacterController _character, PlayerInfo _playerinfo)
	{
		character = _character;
		playerinfo = _playerinfo;
	}



	void ApplyJumping ()
	{
		// Prevent jumping too fast after each other
		if (lastJumpTime + jumpRepeatTime > Time.time)
			return;

		if (IsGrounded() && pushjump == true) {
			// Jump
			// - Only when pressing the button down
			// - With a timeout so you can press the button slightly before landing	
			if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) {
				Debug.Log("je jump");
				verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
				DidJump();
				playerinfo.State = AnimatorState.JumpStart;
			}
			return;
		}
		if (Did_Hit_Wall() && !IsGrounded() && pushjump == true )
		{
			WallJumpCurrentTime = Time.deltaTime;
			DidJump();
			verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
		}
		Debug.Log (IsGrounded ());
	}
	
	void ApplyGravity ()
	{
		if (isControllable && didApplyGravity == true)	// don't move player at all if not controllable.
		{		
			// When we reach the apex of the jump we send out a message
			if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
			{
				jumpingReachedApex = true;
				jumping = false;
				playerinfo.State = AnimatorState.Fall;
				character.SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
			}
			
			
			if (IsGrounded ())
			{
				verticalSpeed = 0.0f;
				if (playerinfo.State != AnimatorState.Land && playerinfo.State != AnimatorState.StopLand )
					playerinfo.State = AnimatorState.Land;
				else
					playerinfo.State = AnimatorState.StopLand;
			}
			else if ((collisionFlags & CollisionFlags.Sides) !=0 && jumpingReachedApex == true)
				verticalSpeed -= gravityWall * Time.deltaTime;
			else
			{
				verticalSpeed -= gravity * Time.deltaTime;
			}
		}
	}
	
	float CalculateJumpVerticalSpeed (float targetJumpHeight)
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * targetJumpHeight * gravity);
	}
	
	void DidJump ()
	{
		jumping = true;
		jumpingReachedApex = false;
		pushjump = false;
		lastJumpTime = Time.time;
		lastJumpStartHeight = character.transform.position.y;
		lastJumpButtonTime = -10;
		
		//	_characterState = CharacterState.Jumping;
	}
	
	float GetSpeed () {
		return moveSpeed;
	}
	
	bool IsJumping () {
		return jumping;
	}
	
	bool IsGrounded () {
		//return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
		return Physics.Raycast(character.transform.position, -Vector3.up, 0.1f);
	}
	
	Vector3 GetDirection () {
		return moveDirection;
	}
	
	bool IsMovingBackwards () {
		return movingBack;
	}
	
	
	bool HasJumpReachedApex ()
	{
		return jumpingReachedApex;
	}
	
	bool IsGroundedWithTimeout ()
	{
		return lastGroundedTime + groundedTimeout > Time.time;
	}
	
	public void jump()
	{
			lastJumpButtonTime = Time.time;
			pushjump = true;
			Debug.Log ("Jump");
	}
	
	public void UpdateMovement(float horizontal, float veritcal)
	{
		ApplyGravity ();
		ApplyJumping ();
	//	Did_Hit_Climb ();
		Apply_Wall_Jump ();
		Apply_wall_climb ();
		Vector3 movement = new Vector3 (
			horizontal * trotSpeed * Time.deltaTime, 
			verticalSpeed, 
			0);
		collisionFlags = character.Move(movement);
		playerinfo.movement = horizontal;
	}

	bool Did_Hit_Wall()
	{
		Vector3 right = character.transform.TransformDirection(Vector3.right);
		Vector3 Left = character.transform.TransformDirection(Vector3.left);
		RaycastHit hit;
		if ( Physics.Raycast (character.transform.position, right, out hit, 0.8f))
		{
			if (WallJumpDirection == 1)
				WallJumpCurrentTime = -1.0f;
			WallJumpDirection = -1;
			return true;
		}
		else if ( Physics.Raycast (character.transform.position, Left, out hit, 0.8f))
		{
			if (WallJumpDirection == -1)
				WallJumpCurrentTime = -1.0f;
			WallJumpDirection = 1;
			return true;
		}
		return false;
	}

	bool Did_Hit_Climb()
	{
		RaycastHit hit;
		Vector3 Direction = Vector3.right + Vector3.down;
		Vector3 Head_cast =  this.character.transform.FindChild("Head_wall").transform.position;
		Debug.DrawRay (Head_cast, Direction * 2, Color.green,  0.2f);
		if (Physics.Raycast (Head_cast, Direction, out hit, 1.5f)) {
			if (hit.normal == Vector3.up) {
				didApplyGravity = false;
				verticalSpeed = 0.0f;
				WallClimbCurrentTime = Time.deltaTime;
			}


		}
		return false;
	}

	void Apply_wall_climb ()
	{
		if (WallClimbCurrentTime != -1.0f)
		{
			if (WallClimbCurrentTime < WallClimbUpDuration)
			{
				Vector3 Movement = new Vector3(0, 4 * Time.deltaTime , 0);
				character.Move(Movement);
				WallClimbCurrentTime += Time.deltaTime;
			}
			else if (WallClimbCurrentTime < WallClimbSideDuratin + WallClimbUpDuration)
			{
				Vector3 Movement = new Vector3(-WallJumpDirection * 8 * Time.deltaTime, 0 , 0);
				character.Move(Movement);
				WallClimbCurrentTime += Time.deltaTime;
			}
			else
			{
				didApplyGravity = true;
				WallClimbCurrentTime = -1.0f;
			}
		}

	}
	
	void Apply_Wall_Jump()
	{
		if (WallJumpCurrentTime != -1.0f)
		{
			if ( WallJumpCurrentTime < WallJumpDuration)
			{
				WallJumpCurrentTime += Time.deltaTime;
				Vector3 Movement = new Vector3(WallJumpDirection * 8 * Time.deltaTime, 0, 0);
				character.Move(Movement);
			}
			else
			{
				WallJumpCurrentTime = -1.0f;
				WallJumpDirection = 0;
			}
		}
	}

}

