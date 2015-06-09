using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;

public class WeaponManager : uLink.MonoBehaviour {
	private List<Iweapon> weapon;
	public GameObject TargetIK;
	public GameObject Riffle;
	public GameObject RiffleParticle;
	public GameObject Gun;
	public GameObject GunParticle;
	public GameObject Shotgun;
	public GameObject ShotgunParticle;
	public GameObject Sword;
	public GameObject SwordParticle;
	private AimIK aimik;
	private int current;
	private int lastcurrent;
	private Vector3 Origin;
	// Use this for initialization

	void Start () {
		weapon = new List<Iweapon>();
		weapon.Add (new Gun (this.gameObject, GunParticle, this.GetComponent<Rewinder> ()));
		weapon.Add (new ShotGun (this.gameObject, GunParticle, this.GetComponent<Rewinder> ()));
		weapon.Add (new MachinGun (this.gameObject, GunParticle, this.GetComponent<Rewinder> ()));
		weapon.Add (new Gun (this.gameObject, GunParticle, this.GetComponent<Rewinder> ()));
		current = 0;
		lastcurrent = 0;
		Gun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = true;
		Shotgun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
		Riffle.transform.FindChild("Mesh").GetComponent<SkinnedMeshRenderer>().enabled = false;
		Sword.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
		aimik = this.transform.FindChild ("Skeleton").GetComponent<AimIK>();
		this.aimik.solver.transform = Gun.transform.FindChild("Ik").transform;
		Origin = Gun.transform.FindChild("Ik").gameObject.transform.position;
	}
	

	public void Shoot(object[] tempStorage)
	{
		double time = (double)tempStorage [0];
		bool shoot = (bool)tempStorage [1];
		if (this.networkView.isProxy)
			weapon[current].ProxyShoot(shoot);
		else if (this.networkView.isOwner)
			weapon[current].ClientShoot (shoot);
		else 
			weapon[current].ServerShoot(TargetIK.transform.position, Origin, (uLink.NetworkTime.serverTime - time - 0.01), shoot);
	}

	public void Reload()
	{
		if (this.networkView.isProxy)
			weapon[current].ProxyReload();
		else if (this.networkView.isOwner)
			weapon[current].ClientReload ();
		else 
			weapon[current].ServerReload();
	}

	void FixedUpdate () {
		//the dead dont shoot
		/*if (playerInfo.dead)
		{
			Gun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
			Shotgun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
			Riffle.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
			Sword.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
			return;
		}*/
		if (this.networkView.isProxy)
			current = this.GetComponent<ProxyPlayer>().PlayerState.weapon;
		else if (this.networkView.isOwner)
			current = this.GetComponent<ClientPlayer>().PlayerState.weapon;
		else 
			current = this.GetComponent<ServerPLayer>().PlayerState.weapon;
		this.transform.FindChild ("Skeleton").GetComponent<Animator>().SetInteger("Weapon", current);
		switch (current) {
		case 0:
			this.aimik.enabled = true;
			Origin = Gun.transform.FindChild("Ik").transform.position;
			this.aimik.solver.transform = Gun.transform.FindChild("Ik").transform;
			this.aimik.solver.axis = new Vector3(0,1,0);
			Gun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = true;
			Shotgun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
			Riffle.transform.FindChild("Mesh").GetComponent<SkinnedMeshRenderer>().enabled = false;
			Sword.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
			break;
		case 1:
			this.aimik.enabled = true;
			this.aimik.solver.axis = new Vector3(-1,0,0);
			Origin = Shotgun.transform.FindChild("Ik").transform.position;
			this.aimik.solver.transform = Shotgun.transform.FindChild("Ik").transform;
			Gun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
			Shotgun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = true;
			Riffle.transform.FindChild("Mesh").GetComponent<SkinnedMeshRenderer>().enabled = false;
			Sword.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
			break;
		case 2:
			this.aimik.enabled = true;
			this.aimik.solver.axis = new Vector3(-1,0,0);
			Origin = Riffle.transform.FindChild("Ik").transform.position;
			this.aimik.solver.transform = Riffle.transform.FindChild("Ik").transform;
			Gun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
			Shotgun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
			Riffle.transform.FindChild("Mesh").GetComponent<SkinnedMeshRenderer>().enabled = true;
			Sword.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
			break;
		case 3:
			this.aimik.enabled = false;
//			Origin = Sword.transform.FindChild("Ik").transform.position;
//			this.aimik.solver.transform = Sword.transform.FindChild("Ik").transform;
			Gun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
			Shotgun.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = false;
			Riffle.transform.FindChild("Mesh").GetComponent<SkinnedMeshRenderer>().enabled = false;
			Sword.transform.FindChild("Mesh").GetComponent<MeshRenderer>().enabled = true;
			break;
		}
	}

	public void instantiate(GameObject part)
	{
		Vector3 direction = TargetIK.transform.position - Origin;
		GameObject tmp = (GameObject)Instantiate(part, Origin,  Quaternion.LookRotation(direction));
		Destroy(tmp, 0.5f);
	}

	public void changeWeapon (float i)
	{
		if (i > 0) {
			this.current += 1;
			if (this.current > weapon.Count - 1)
				this.current = 0;
		} else if (i < 0) {
			this.current -= 1;
			if (this.current < 0)
				this.current = weapon.Count - 1;
		}
		this.GetComponent<ServerPLayer> ().PlayerState.weapon = current;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
