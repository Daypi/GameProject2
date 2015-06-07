using UnityEngine;
using System.Collections;

public class Gun : Iweapon {
	private float fireDelay;
	private float timeSinceLastShoot;
	private int ammo = 10;
	private int damage = 20;
	private GameObject Owner;
	private GameObject Particule;
	private Rewinder rewinder;
	public Gun(GameObject _owner, GameObject _particule, Rewinder _rewinder){ Owner = _owner; Particule = _particule; rewinder = _rewinder;}
	public void ServerShoot(Vector3 target, Vector3 origin, double time){
		if (Time.time > fireDelay + timeSinceLastShoot) { //&& ammo != 0)
			Vector3 direction = (target - origin).normalized;
			Debug.DrawRay (origin, direction, Color.red, 5.0f);
			RaycastHit hit;
			if (rewinder.Raycast (origin, direction, out hit, time)) {
				Collider TargetHit = hit.collider;
				Debug.Log (TargetHit);
				if (TargetHit.tag == "Player")
					;
				//sendRpC life
			}
		}
	}
	public void ClientShoot(){return;}
	public void ProxyShoot(){return;}
}
