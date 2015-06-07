using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientPlayer : uLink.MonoBehaviour {

	public Queue<StructCodec.InputStruct> inputarray;
	public CharacterMotor motor;
	public LayerMask mask = -1;
	public int CharacterMesh = 0;
	StructCodec.PlayerStateStruct PlayerState = new StructCodec.PlayerStateStruct();

	void uLink_OnNetworkInstantiate (uLink.NetworkMessageInfo info ) { 
		System.RuntimeTypeHandle toto = new System.RuntimeTypeHandle();
		PlayerState.nickname = (string)info.networkView.initialData.ReadObject(typeof(string).TypeHandle);
		CharacterMesh = (int)info.networkView.initialData.ReadObject(typeof(int).TypeHandle);
	} 

	// Use this for initialization
	void Start () {
		inputarray = new Queue<StructCodec.InputStruct> ();
		motor = new CharacterMotor(this.GetComponent<CharacterController>());
		Physics.IgnoreLayerCollision(8, 8);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		StructCodec.InputStruct inputstruct = new StructCodec.InputStruct ();
		inputstruct.ID = (float)uLink.NetworkTime.serverTime;
		inputstruct.hor = Input.GetAxis ("Horizontal");
		inputstruct.jump = Input.GetButtonDown ("Jump");
		inputstruct.shoot = Input.GetButton ("Fire1");
		inputstruct.dt = Time.fixedDeltaTime;
		inputstruct.aimpos = this.transform.FindChild ("Aim").transform.position;
		uLink.NetworkView.Get (this).RPC ("UpdateInput", uLink.RPCMode.Server, inputstruct);
		if (inputstruct.jump == true)
			motor.jump (Time.fixedTime);
		motor.UpdateMovement (inputstruct.hor, Time.fixedDeltaTime);
		inputstruct.result = this.transform.position;
		inputarray.Enqueue (inputstruct);
		if (this.transform.FindChild ("Aim").transform.localPosition.x < 0) {
			PlayerState.facing = false;
			this.transform.FindChild("Skeleton").transform.localRotation = Quaternion.Euler(0,-90,0);
		} else
		{
			PlayerState.facing = true;
			this.transform.FindChild("Skeleton").transform.localRotation = Quaternion.Euler(0,90,0);
		}
		if (Input.GetMouseButton (0)) {
			uLink.NetworkView.Get (this).RPC ("Shoot", uLink.RPCMode.Server);
		}
	}
}
