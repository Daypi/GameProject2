using UnityEngine;
using System.Collections;


public class PlayerManager : MonoBehaviour {
    public NetworkPlayer owner;
	private CharacterController controller;
	private float horizontalMotion;
	private float verticalMotion;
	private PlayerInfo playerInfo;
	MovementGestion movement;

	// Use this for initialization
	void Start () {
		if (Network.isServer) {
			playerInfo = this.GetComponent<PlayerInfo>();
			controller = this.GetComponent<CharacterController>();
			movement = new MovementGestion(controller, playerInfo);
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
	void Update () {
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
        if (movement != null)
		    movement.UpdateMovement (horizontalMotion, verticalMotion);
        //this.GetComponentInChildren<RectTransform>().anchoredPosition = this.transform.localPosition;
	}
	

	/**
     * The client calls this to notify the server about Jump button push
     * @param
     */
	[RPC]
	public void jump()
	{
		Debug.Log ("rpcJump");
		movement.jump ();
	}


	/**
     * The client calls this to notify the server about new motion data
     * @param	motion
     */
	[RPC]
	public void updateClientMotion(float hor, float vert)
	{
		horizontalMotion = hor;
		verticalMotion = vert;
		Vector3 position = this.transform.position;
		position.z = 0;
		this.transform.position = position;
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
