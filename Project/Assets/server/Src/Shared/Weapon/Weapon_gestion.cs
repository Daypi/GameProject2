using UnityEngine;
using System.Collections.Generic;
using RootMotion.FinalIK;

public class Weapon_gestion : MonoBehaviour {

	public GameObject Riffle;
	public GameObject RiffleParticle;
	public GameObject Gun;
	public GameObject GunParticle;
	public GameObject Shotgun;
	public GameObject ShotgunParticle;
	public GameObject Sword;
	public GameObject SwordParticle;
	private AimIK aimik;
	public GameObject targetIK;
	private List<Iweapon> weapon;
	private int current;
	private int lastcurrent;
	private PlayerInfo playerInfo;
	private Vector3 currentChild;


	// Use this for initialization
	public Weapon_gestion (GameObject owner, PlayerInfo player) {
	}

	void Start () {
		weapon = new List<Iweapon>();
		weapon.Add (new Gun(this.gameObject, GunParticle));
		weapon.Add (new Shotgun(this.Shotgun));
		weapon.Add (new Rifle(this.Riffle));
		playerInfo = this.GetComponent<PlayerInfo>();
		current = 0;
		lastcurrent = 0;
		Gun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = true;
		Shotgun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
		Riffle.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
		Sword.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
		aimik = this.transform.FindChild ("mixamo_walk").GetComponent<AimIK>();
		this.aimik.solver.transform = Gun.transform.FindChild("Ik").transform;
		currentChild = Gun.transform.FindChild("Ik").gameObject.transform.position;
	}

	void FixedUpdate () {
        //the dead dont shoot
        if (playerInfo.dead)
        {
            Gun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
            Shotgun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
            Riffle.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
            Sword.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
            return;
        }
		current = playerInfo.gun;
			lastcurrent = current;
			switch (current) {
			case 0:
				currentChild = Gun.transform.FindChild("Ik").transform.position;
				this.aimik.solver.transform = Gun.transform.FindChild("Ik").transform;
				Gun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = true;
				Shotgun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
				Riffle.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
				Sword.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
				break;
			case 1:
				currentChild = Shotgun.transform.FindChild("Ik").transform.position;
				this.aimik.solver.transform = Shotgun.transform.FindChild("Ik").transform;
				Gun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
				Shotgun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = true;
				Riffle.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
				Sword.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
				break;
			case 2:
				currentChild = Riffle.transform.FindChild("Ik").transform.position;
				this.aimik.solver.transform = Riffle.transform.FindChild("Ik").transform;
				Gun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
				Shotgun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
				Riffle.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = true;
				Sword.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
				break;
			case 3:
				currentChild = Sword.transform.FindChild("Ik").transform.position;
				this.aimik.solver.transform = Sword.transform.FindChild("Ik").transform;
				Gun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
				Shotgun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
				Riffle.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
				Sword.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = true;
				break;
		}
	}

	public void C_Shoot()
	{
        if (playerInfo.dead)
            return;
		/*switch (current) {
		case 0:
			Vector3 direction = targetIK.transform.position - currentChild;
			GameObject tmp = (GameObject)Instantiate(GunParticle, currentChild,  Quaternion.LookRotation(direction));
                Destroy(tmp, 0.5f);
			break;
		}*/
		weapon [0].C_shoot ();
	}

	public void instantiate(GameObject part)
	{
		Vector3 direction = targetIK.transform.position - currentChild;
		GameObject tmp = (GameObject)Instantiate(part, currentChild,  Quaternion.LookRotation(direction));
		Destroy(tmp, 0.5f);
	}

	
	[RPC]
	public void shoot()
	{
        //dead ppl dont shoot
        if (playerInfo.dead)
            return;
		weapon[0].shoot (targetIK, currentChild);
	/*	Vector3 vForce = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
		vForce.Normalize ();
		Debug.DrawRay (weapon[0].owner.transform.position, vForce, Color.red, 2.0f);*/
	}

	[RPC]
	public void changeWeapon(float value)
	{
		this.current += 1;
		if (this.current > weapon.Count - 1)
			this.current = 0;
		playerInfo.gun = this.current;
	}
}
