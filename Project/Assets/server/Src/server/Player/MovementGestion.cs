using UnityEngine;
using System.Collections;

public class MovementGestion {

	CharacterController character;
	// The speed when walking
	float walkSpeed = 2.0f;
	// after trotAfterSeconds of walking we trot with trotSpeed
	float trotSpeed = 4.0f;
	// when pressing "Fire3" button (cmd) we start running
	float runSpeed = 6.0f;
	
	float inAirControlAcceleration = 3.0f;
	
	// How high do we jump when pressing jump and letting go immediately
	float jumpHeight = 0.1f;
	
	// The gravity for the character
	float gravity = 1.0f;
	// The gravity in controlled descent mode
	float speedSmoothing = 10.0f;
	float rotateSpeed = 500.0f;
	float trotAfterSeconds = 3.0f;
	
	bool canJump = true;
	private bool pushjump = false;

	private float jumpRepeatTime = 0.05f;
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
	
	
	// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
	private float lastJumpStartHeight = 0.0f;
	
	
	private Vector3 inAirVelocity = Vector3.zero;
	
	private float lastGroundedTime = 0.0f;
	
	
	private bool isControllable = true;

	public MovementGestion(CharacterController _character)
	{
		character = _character;
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
				verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
				DidJump();
			}
		}
	}

	void ApplyGravity ()
	{
		if (isControllable)	// don't move player at all if not controllable.
		{		
			// When we reach the apex of the jump we send out a message
			if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
			{
				jumpingReachedApex = true;
				character.SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
			}


			if (IsGrounded ())
				verticalSpeed = 0.0f;
			else
				verticalSpeed -= gravity * Time.deltaTime;
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
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
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
	}

	public void UpdateMovement(float horizontal, float veritcal)
	{
		ApplyGravity ();
		ApplyJumping ();
		Vector3 movement = new Vector3 (
			horizontal * trotSpeed * Time.deltaTime, 
			verticalSpeed, 
			veritcal * trotSpeed * Time.deltaTime);
			collisionFlags = character.Move(movement);
	}

}
