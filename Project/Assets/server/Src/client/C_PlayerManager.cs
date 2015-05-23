using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;
using System.Collections.Generic;

public class C_PlayerManager : MonoBehaviour {
	
	public GameObject child;
	//That's actually not the owner but the player,
    //the server instantiated the prefab for, where this script is attached
	public NetworkPlayer owner;
    public string guid;
    //Those are stored to only send RPCs to the server when the 
    //data actually changed.
    private float lastMotionH; //horizontal motion
    private float lastMotionV; //vertical motion
	private MovementGestion move;
	public Vector3 serverPos;
	public Quaternion serverRot;
	//public Quaternion serverRot;
	public float positionErrorThreshold = 0.2f;
	public float positionErrorThresholdDanger = 2.0f;
	public CircularBuffer<PosTime> predictionresult;
	public bool Ismine = false;
    
	[RPC]
    void setOwner(NetworkPlayer player, string aguid)
	{
		Debug.Log("Setting the owner. owner.guid=" + player.guid);
		owner = player;
        guid = aguid;
		if(player == Network.player){
			//So it just so happens that WE are the player in question,
			//which means we can enable this control again
			enabled=true;
			Ismine = true;
			this.transform.FindChild("IkAim").GetComponent<NetworkView>().observed = null;
			move = new MovementGestion(this.GetComponent<CharacterController>(), this.GetComponent<PlayerInfo>());
		}
		else {
			Ismine = false;
			//Disable a bunch of other things here that are not interesting:
			if (this.transform.FindChild("Main Camera").GetComponent<Camera>()) {
				this.transform.FindChild("Main Camera").GetComponent<Camera>().enabled = false;
			}
	if (Network.isClient)
			this.transform.FindChild("IkAim").GetComponent<FollowMouse>().enabled = false;

			if (this.transform.FindChild("Main Camera").GetComponent<AudioListener>()) {
				this.transform.FindChild("Main Camera").GetComponent<AudioListener>().enabled = false;
			}
			
			if (this.transform.FindChild("Main Camera").GetComponent<GUILayer>()) {
				this.transform.FindChild("Main Camera").GetComponent<GUILayer>().enabled = false;
			}
		}
	}


	[RPC]
	public NetworkPlayer getOwner() {
		return owner;
	}

	public void Awake() {
		//Disable this by default for now
		//Just to make sure no one can use this until we didn't
		//find the right player. (see setOwner())
		if (Network.isClient) {
            ;// enabled = false;
		}
	}

	// Use this for initialization
	void Start () {
		predictionresult = new CircularBuffer<PosTime> (500);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!this.GetComponent<PlayerInfo>().dead && !this.GetComponent<HpBar>().enabled)
            this.GetComponent<HpBar>().enabled = true;
        else if (this.GetComponent<PlayerInfo>().dead && this.GetComponent<HpBar>().enabled)
            this.GetComponent<HpBar>().enabled = false;
        this.GetComponent<HpBar>().scale = this.GetComponent<PlayerInfo>().Hp / 100;
        //si on est mort, on ne s'affiche pas
        if (this.GetComponent<PlayerInfo>().dead)
        {
            Renderer[] lChildRenderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer lRenderer in lChildRenderers)
                lRenderer.enabled = false;
            gameObject.GetComponentInChildren<CharacterController>().enabled = false;
        }
		if (Network.isServer) {
			return; //get lost, this is the client side!
		}
		//Check if this update applies for the current client
		if ((owner != null) && (owner == Network.player)) {
			//Debug.Log("moving");
            //Debug.Log("hp du client :" + this.GetComponent<PlayerManager>().Hp);
            
			/*if (Input.GetKeyDown("space"))
			{
				Debug.Log ("clientJump");
				GetComponent<NetworkView>().RPC("jump",RPCMode.Server);
				move.jump(Time.fixedTime);
			}
			float motionH  = Input.GetAxis("Horizontal");
			float motionV  = 0;
			if ((motionH != lastMotionH) || (motionV != lastMotionV)) {
				GetComponent<NetworkView>().RPC("updateClientMotion", 
				                RPCMode.Server, 
				                Input.GetAxis("Horizontal"), 
				                Input.GetAxis("Vertical"));
				lastMotionH = motionH;
				lastMotionV = motionV;
			}
			if (Input.GetMouseButtonDown (0))
			{
				GetComponent<NetworkView>().RPC("shoot",RPCMode.Server);
				GetComponent<Weapon_gestion>().C_Shoot();
			}

			float mousewheel = Input.GetAxis("Mouse ScrollWheel");
				if (mousewheel != 0)
			{
				GetComponent<NetworkView>().RPC("changeWeapon",RPCMode.Server, mousewheel);
			}
			move.UpdateMovement(motionH, Time.fixedDeltaTime);*/

			Inputstruct inputi = new Inputstruct(0,0,false,false, (float)Network.time);
			//Debug.Log (Network.time);
			if (Input.GetKeyDown("space"))
			{
				inputi.Jump = true;
				move.jump(Time.fixedTime);
			}
			if (Input.GetButton("Fire1"))
			{
				inputi.Shoot = true;
				GetComponent<Weapon_gestion>().C_Shoot();
			}
			inputi.Horizontal = Input.GetAxis("Horizontal");
			GetComponent<NetworkView>().RPC("updateInput",RPCMode.Server, inputi.Horizontal, inputi.Jump, inputi.ExecuteTime, inputi.Shoot);
			move.UpdateMovement(inputi.Horizontal, Time.deltaTime);
			inputi.dt = Time.deltaTime;
			predictionresult.Enqueue(new PosTime(this.transform.position, inputi.ExecuteTime, inputi, 0));
		}
	}

	public void lerpToTarget() {
		var distance = Vector3.Distance(transform.position, serverPos);
		
		//only correct if the error margin (the distance) is too extreme
	/*	if (distance >= positionErrorThresholdDanger) {
			this.transform.position = serverPos;
		}*/
		/*if (distance >= positionErrorThreshold) {
			float lerp = ((1 / distance) * move.trotSpeed) / 100;
			//Debug.Log("Lerp time: " + lerp);
			transform.position = Vector3.Lerp(transform.position, serverPos, lerp);
			Debug.Log ("serverpos: " + serverPos + "this pos : " + transform.position);
		//	transform.rotation = Quaternion.Slerp(transform.rotation, serverRot, lerp);
		}*/
	}


}
