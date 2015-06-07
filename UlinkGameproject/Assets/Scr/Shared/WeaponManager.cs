using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : uLink.MonoBehaviour {
	private List<Iweapon> weapon;
	public GameObject TargetIK;
	// Use this for initialization
	void Start () {
		weapon = new List<Iweapon>();
		weapon.Add (new Gun (this.gameObject, null, this.GetComponent<Rewinder> ()));
	}

	[RPC]
	public void Shoot(uLink.NetworkMessageInfo info)
	{
		weapon[0].ServerShoot(this.transform.position, TargetIK.transform.position, (uLink.NetworkTime.serverTime - info.elapsedTimeSinceSent - 0.01));
	}


	// Update is called once per frame
	void Update () {
	
	}
}
