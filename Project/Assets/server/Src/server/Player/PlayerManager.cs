﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerManager : MonoBehaviour {
    public NetworkPlayer owner;
	private CharacterController controller;
	private float horizontalMotion;
	private float verticalMotion;
	private PlayerInfo playerInfo;
	MovementGestion movement;
	Queue<Inputstruct> inputarray;
//	public CircularBuffer<PosTime> result;
	public PosTime lastResult;
	Inputstruct lastinput = new Inputstruct(0f,0f,false,false,0);

	// Use this for initialization
	void Start () {
		if (Network.isServer) {
			playerInfo = this.GetComponent<PlayerInfo>();
			controller = this.GetComponent<CharacterController>();
			movement = new MovementGestion(controller, playerInfo);
			inputarray = new Queue<Inputstruct>();
		//	result = new CircularBuffer<PosTime>(100);
			lastResult = new PosTime(Vector3.up, 0, lastinput, 0);
		}
	}
	
    void respawn()
    {
        int selectedId;
        GameObject[] respawns = GameObject.FindGameObjectsWithTag("Respawn");
        selectedId = Random.Range(0, respawns.Length);
        this.transform.position = respawns[selectedId].transform.position;
        playerInfo.dead = false;
    }

	// Update is called once per frame
	void FixedUpdate () {
		if (Network.isClient) {
			return; //Get lost, this is the server-side!
		}
        //si on est mort, il faut mourrir
        if (playerInfo.Hp <= 0)
        {
            //reset les valeurs du joueur
            playerInfo.dead = true;
            playerInfo.resetPlayerData();
            this.respawn();
            //envoyer un RPC a tout le monde (pour afficher le kill)
            //timer avant de respawn
        }
		//Debug.Log("Processing clients movement commands on server");
       /* if (movement != null)
		    movement.UpdateMovement (horizontalMotion, Time.fixedDeltaTime);*/
		if (inputarray.Count != 0) {
			Inputstruct curr = inputarray.Dequeue ();
			lastinput = curr;
			if (curr.Jump == true) {
				movement.jump (Time.fixedTime);
			}
			if (curr.Shoot == true)
				this.GetComponent<Weapon_gestion> ().shoot ();
			movement.UpdateMovement (curr.Horizontal, Time.fixedDeltaTime);
			lastResult =  new PosTime (this.transform.position, curr.ExecuteTime, curr, Network.time);
		} else 
		{
			Inputstruct curr = lastinput;
			if (curr.Jump == true) {
				movement.jump (Time.fixedTime);
			}
			if (curr.Shoot == true)
				this.GetComponent<Weapon_gestion> ().shoot ();
			movement.UpdateMovement (curr.Horizontal, Time.fixedDeltaTime);
		}
        //this.GetComponentInChildren<RectTransform>().anchoredPosition = this.transform.localPosition;
	}



	[RPC]
	public void updateInput(float _hor, bool _jump, float time, bool shoot,  NetworkMessageInfo info)
	{
		inputarray.Enqueue(new Inputstruct(_hor, 0.0f, _jump, shoot, time));
	}

	
    public void takeDamage(int damage)
    {
		playerInfo.Hp -= damage;
		Debug.Log ("Dammage: " + damage + " Hp player: " + playerInfo.Hp); 
		if (playerInfo.Hp < 0) {
			playerInfo.Hp = 0;
		}
    }

}
