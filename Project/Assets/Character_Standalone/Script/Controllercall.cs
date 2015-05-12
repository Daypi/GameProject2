using UnityEngine;
using System.Collections;

public class Controllercall : MonoBehaviour {

	MovementGestion move;
	// Use this for initialization
	void Start () {
		move = new MovementGestion(this.GetComponent<CharacterController>(), this.GetComponent<PlayerInfo>());
	}
	
	// Update is called once per frame
	void Update () {
		move.UpdateMovement (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		if (Input.GetButtonDown("Jump"))
		    move.jump();
		if (Input.GetButtonDown ("Fire1"))
			this.transform.Find ("mixamo_walk").GetComponent<Animator> ().SetBool ("Shoot", true);
	}
}
