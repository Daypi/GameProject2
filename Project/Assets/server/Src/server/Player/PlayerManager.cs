using UnityEngine;
using System.Collections;


public class PlayerManager : MonoBehaviour {
	public GameObject ShootGunpParticle;
	public GameObject GunParticle;
	public GameObject FlamethrowerParticle;
    public NetworkPlayer owner;
	private CharacterController controller;
	private float horizontalMotion;
	private float verticalMotion;
    public float Hp;
	MovementGestion movement;
	Weapon_gestion weaponG;

	// Use this for initialization
	void Start () {
        Hp = 100f;
		if (Network.isServer) {
			controller = this.GetComponent<CharacterController>();
			movement = new MovementGestion(controller);
			weaponG = new Weapon_gestion(this.gameObject, ShootGunpParticle,GunParticle,FlamethrowerParticle);
		}
	}
	

	// Update is called once per frame
	void Update () {
		if (Network.isClient) {
			return; //Get lost, this is the server-side!
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
		movement.jump ();
	}

	/**
     * The client calls this to notify the server about Shoot button push
     * @param	angle
     */
	[RPC]
	public void shoot(float angle)
	{
		weaponG.shoot (angle);
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
	}

    public void takeDamage(int damage)
    {
        this.Hp -= damage;
        if (this.Hp < 0)
            this.Hp = 0;

        GetComponent<NetworkView>().RPC("setHp", RPCMode.Others, this.Hp, this.GetComponent<C_PlayerManager>().owner.guid);
    }
}
