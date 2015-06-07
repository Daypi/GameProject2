using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerPLayer : uLink.MonoBehaviour {
	public StructCodec.ResultStruct LastResult;
	Queue<StructCodec.InputStruct> inputarray;
	CharacterMotor Motor;
	// Use this for initialization
	void Start () {
		Motor = new CharacterMotor (this.GetComponent<CharacterController> ());
		inputarray = new Queue<StructCodec.InputStruct> ();
		LastResult = new StructCodec.ResultStruct ();
		Physics.IgnoreLayerCollision(8, 8);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (inputarray.Count != 0) {
			StructCodec.InputStruct CurrentInput = inputarray.Dequeue ();
			if (CurrentInput != null) {
				if (CurrentInput.jump == true)
					Motor.jump (Time.fixedTime);
				Motor.UpdateMovement (CurrentInput.hor, Time.fixedDeltaTime);
				LastResult.ID = CurrentInput.ID;
				this.transform.FindChild ("Aim").transform.position = CurrentInput.aimpos;
				LastResult.aimpos = CurrentInput.aimpos;
				LastResult.position = this.transform.position;
				if (this.transform.FindChild ("Aim").transform.localPosition.x < 0) {
					this.transform.FindChild ("Skeleton").transform.localRotation = Quaternion.Euler (0, -90, 0);
				} else {
					this.transform.FindChild ("Skeleton").transform.localRotation = Quaternion.Euler (0, 90, 0);
				}
			}
		}
		this.GetComponent<Rewinder> ().AddRewinderstruct (new RewinderStruct (this.transform.position, uLink.NetworkTime.serverTime));
	}

	[RPC]
	void UpdateInput(StructCodec.InputStruct input)
	{
		inputarray.Enqueue (input);
	}
}
