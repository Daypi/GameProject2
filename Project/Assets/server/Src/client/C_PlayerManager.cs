using UnityEngine;
using System.Collections;

public class C_PlayerManager : MonoBehaviour {

	public float angle;
	private float character_angle;
    public float hp;
	public GameObject child;
	//That's actually not the owner but the player,
    //the server instantiated the prefab for, where this script is attached
	public NetworkPlayer owner;
    public string guid;
    //Those are stored to only send RPCs to the server when the 
    //data actually changed.
    private float lastMotionH; //horizontal motion
    private float lastMotionV; //vertical motion
    
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
		}
		else {
			//Disable a bunch of other things here that are not interesting:
			if (this.transform.FindChild("Main Camera").GetComponent<Camera>()) {
				this.transform.FindChild("Main Camera").GetComponent<Camera>().enabled = false;
			}

			if (this.transform.FindChild("Main Camera").GetComponent<AudioListener>()) {
				this.transform.FindChild("Main Camera").GetComponent<AudioListener>().enabled = false;
			}
			
			if (this.transform.FindChild("Main Camera").GetComponent<GUILayer>()) {
				this.transform.FindChild("Main Camera").GetComponent<GUILayer>().enabled = false;
			}
		}
	}

    [RPC]
    public void setHp(float hparg, string aguid)
    {
        Debug.Log("son sien" + aguid);
        Debug.Log("mon mien :" + guid);
        if (aguid == guid)
        {
            this.hp = hparg;
            Debug.Log(hparg);
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
			enabled = false;
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Network.isServer) {
			return; //get lost, this is the client side!
		}
		//Check if this update applies for the current client
		if ((owner != null) && (owner == Network.player)) {
			//Debug.Log("moving");
            //Debug.Log("hp du client :" + this.GetComponent<PlayerManager>().Hp);
            this.GetComponentInChildren<HpBar>().scale = hp / 100;
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
			if (Input.GetButton("Jump"))
			{
				child.SendMessage("jumpanim");
				GetComponent<NetworkView>().RPC("jump",RPCMode.Server);
			}
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 10;
			Vector3 mouse = Camera.main.GetComponent<Camera>().ScreenToWorldPoint  (mousePos);
			GameObject bones = GameObject.Find ("ak");
			this.angle = (GetAngle (this.transform.position, mouse));
			this.character_angle = (GetAngle (this.transform.position, mouse));
			if (Input.GetMouseButtonDown (0))
			{
				GetComponent<NetworkView>().RPC("shoot",RPCMode.Server, character_angle);
			}
		}
	}

	float GetAngle(Vector3 from, Vector3 to)
	{
		Vector2 From = new Vector2(from.x, from.y);
		Vector2 To = new Vector2(to.x, to.y) ;
		float angleBetween = Mathf.Atan2(To.y - From.y, To.x - From.x) * 180 / Mathf.PI;
		return angleBetween;
		
	}
	
}
