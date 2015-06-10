﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ClientPlayer : uLink.MonoBehaviour {

	public Queue<StructCodec.InputStruct> inputarray;
	public CharacterMotor motor;
	public GameObject healpart;
	public LayerMask mask = -1;
	public int CharacterMesh = 0;
	public Animator anim;
	public StructCodec.PlayerStateStruct PlayerState = new StructCodec.PlayerStateStruct();
	double timesincelastHeal = 0;
	double Healdelay = 0.5;
	
	void uLink_OnNetworkInstantiate (uLink.NetworkMessageInfo info ) { 
		System.RuntimeTypeHandle toto = new System.RuntimeTypeHandle();
        PlayerState.nickname = (string)info.networkView.initialData.Read<string>();
	} 

	// Use this for initialization
	void Start () {
		anim = this.transform.FindChild("Skeleton").GetComponent<Animator> ();
		inputarray = new Queue<StructCodec.InputStruct> ();
		motor = new CharacterMotor(this.GetComponent<CharacterController>(), anim);
		Physics.IgnoreLayerCollision(8, 8);
	}

	void heal()
	{
		if (Time.time > Healdelay + timesincelastHeal) {
		GameObject tmp = (GameObject)Instantiate (healpart, this.transform.position, Quaternion.Euler (-90, 0, 0));
		Destroy(tmp, 1.0f);
			timesincelastHeal = Time.time;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		StructCodec.InputStruct inputstruct = new StructCodec.InputStruct ();
		inputstruct.ID = (float)uLink.NetworkTime.serverTime;
		if (Input.GetKey ("f")) {
			inputstruct.heal = true;
			inputstruct.hor = 0;
			heal ();
		} else {
			inputstruct.hor = Input.GetAxis ("Horizontal");
			inputstruct.jump = Input.GetButtonDown ("Jump");
			inputstruct.shoot = Input.GetButton ("Fire1");
		}
		inputstruct.dt = Time.fixedDeltaTime;
		inputstruct.aimpos = this.transform.FindChild ("Aim").transform.position;
		if (Input.GetMouseButton (0) && inputstruct.heal != true) {
			inputstruct.shoot = true;
			anim.SetBool("Shoot", true);
		}
		else 
			anim.SetBool("Shoot", false);
		object[] tempStorage = new object[2];
		tempStorage [0] = 0.0;
		tempStorage [1] = inputstruct.shoot;
		SendMessage("Shoot", tempStorage);
		if (Input.GetKey ("r") && inputstruct.heal != true) {
			inputstruct.reload = true;
			anim.SetBool("Reload", true);
			SendMessage("Reload");
		}
		else
			anim.SetBool("Reload", false);
		inputstruct.changeweapon = Input.GetAxis ("Mouse ScrollWheel");
		uLink.NetworkView.Get (this).RPC ("UpdateInput", uLink.RPCMode.Server, inputstruct);
		if (inputstruct.jump == true && inputstruct.heal != true)
			motor.jump (Time.fixedTime);
		motor.UpdateMovement (inputstruct.hor, Time.fixedDeltaTime);
		inputstruct.result = this.transform.position;
		inputarray.Enqueue (inputstruct);
		if (this.transform.FindChild ("Aim").transform.localPosition.x < 0) {
			PlayerState.facing = false;
			this.transform.FindChild("Skeleton").transform.localRotation = Quaternion.Euler(0,-90,0);
			anim.SetFloat ("Movement", inputstruct.hor * -1);
		} else
		{
			anim.SetFloat ("Movement", inputstruct.hor);
			PlayerState.facing = true;
			this.transform.FindChild("Skeleton").transform.localRotation = Quaternion.Euler(0,90,0);
		}
	}
}
