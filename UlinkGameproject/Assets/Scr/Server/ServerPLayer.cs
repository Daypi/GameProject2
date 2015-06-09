﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerPLayer : uLink.MonoBehaviour {
	public StructCodec.ResultStruct LastResult;
	Queue<StructCodec.InputStruct> inputarray;
	CharacterMotor Motor;
	public StructCodec.PlayerStateStruct PlayerState = new StructCodec.PlayerStateStruct();
	// Use this for initialization
	void Start () {
		Motor = new CharacterMotor (this.GetComponent<CharacterController> (), this.transform.FindChild("Skeleton").GetComponent<Animator>());
		inputarray = new Queue<StructCodec.InputStruct> ();
		LastResult = new StructCodec.ResultStruct ();
		PlayerState.life = 100;
		PlayerState.isdead = false;
	}

	void uLink_OnNetworkInstantiate (uLink.NetworkMessageInfo info ) { 
		System.RuntimeTypeHandle toto = new System.RuntimeTypeHandle();
		PlayerState.nickname = (string)info.networkView.initialData.ReadObject(typeof(string).TypeHandle);
	} 

	public void Life(int value)
	{
		this.PlayerState.life += value;
        if (this.PlayerState.life <= 0)
            this.PlayerState.isdead = true;

		Debug.Log (PlayerState.life);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (inputarray.Count != 0) {
			StructCodec.InputStruct CurrentInput = inputarray.Dequeue ();
			if (CurrentInput != null) {
				object[] tempStorage = new object[2];
				tempStorage [0] = CurrentInput.info.elapsedTimeSinceSent;
				tempStorage [1] = CurrentInput.shoot;
				SendMessage("Shoot", tempStorage);
				if (CurrentInput.changeweapon != 0)
					SendMessage("changeWeapon", CurrentInput.changeweapon);
				if (CurrentInput.reload == true)
					SendMessage("Reload");
				if (CurrentInput.jump == true)
					Motor.jump (Time.fixedTime);
				Motor.UpdateMovement (CurrentInput.hor, Time.fixedDeltaTime);
				LastResult.ID = CurrentInput.ID;
				this.transform.FindChild ("Aim").transform.position = CurrentInput.aimpos;
				LastResult.aimpos = CurrentInput.aimpos;
				LastResult.position = this.transform.position;
				LastResult.VertcialSpeed = Motor.verticalSpeed;
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
	void UpdateInput(StructCodec.InputStruct input, uLink.NetworkMessageInfo info)
	{
		input.info = info;
		inputarray.Enqueue (input);
	}
}
