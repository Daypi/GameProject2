using UnityEngine;
using System.Collections;

public class Controllercall : MonoBehaviour {

	MovementGestion move;
	// Use this for initialization
	void Start () {
		move = new MovementGestion(this.GetComponent<CharacterController>(), this.GetComponent<PlayerInfo>());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		move.UpdateMovement (Input.GetAxis ("Horizontal"), Time.fixedDeltaTime);
		if (Input.GetButtonDown("Jump"))
		    move.jump(Time.fixedTime);
		if (Input.GetButtonDown ("Fire1"))
			this.transform.Find ("mixamo_walk").GetComponent<Animator> ().SetBool ("Shoot", true);
	}
}
