using UnityEngine;
using System.Collections;
using uLink;

public class Sword : Iweapon {
	private double fireDelay = 1.0;
	private float timeSinceLastShoot;
	private int initialammo = 0;
	private int ammo = 0;
	private int damage = -70;
	private GameObject Owner;
	private GameObject Particule;
	private Rewinder rewinder;
	bool isreaload = false;
	bool lasstShoot = false;
	float reloadTime = 0;
	float timeSinceReload;
	float distance = 1.50f;
	public Sword(GameObject _owner, GameObject _particule, Rewinder _rewinder){ Owner = _owner; Particule = _particule; rewinder = _rewinder;}
	public void ServerShoot(Vector3 target, Vector3 origin, double time, bool shoot){
		if (isreaload == true) {
			if (Time.time > reloadTime + timeSinceReload)
			{
				isreaload = false;
				ammo = initialammo;
			}
			return;
		}
		origin = Owner.transform.position;
		origin.y = Owner.transform.position.y + 0.5f;
		if (shoot == true && lasstShoot == false) {
			if (Time.time > fireDelay + timeSinceLastShoot) {
				timeSinceLastShoot = Time.time;
				Vector3 direction = (target - origin).normalized;
				Debug.DrawRay (origin, direction, Color.red, 5.0f);
				RaycastHit hit;
				if (rewinder.Raycast (origin, direction, out hit, time, distance)) {
					Collider TargetHit = hit.collider;
					Debug.Log (TargetHit);
					if (TargetHit.tag == "Ghostcollider")
						TargetHit.GetComponentInParent<ServerPLayer> ().Life (damage, this.Owner.GetComponent<ServerPLayer>().PlayerState.nickname, "Gun", this.Owner.GetComponent<ServerPLayer>().PlayerState);

				}
				Owner.GetComponent<ServerPLayer>().SendProxyShoot();
			}
		}
		lasstShoot = shoot;
	}

	public void ServerReload()
	{
		if (isreaload != true) {
			isreaload = true;
			timeSinceReload = Time.time;
		}
	}

	public void ClientReload()
	{
		if (isreaload != true) {
			isreaload = true;
			timeSinceReload = Time.time;
		}
	}

	public void ProxyReload()
	{
	}
	
	public void ClientShoot(bool shoot){
		if (isreaload == true) {
			if (Time.time > reloadTime + timeSinceReload)
			{
				isreaload = false;
				ammo = initialammo;
			}
			return;
		}
		if (shoot == true && lasstShoot == false) {
			if (Time.time > fireDelay + timeSinceLastShoot) {  //&& ammo != 0)
				timeSinceLastShoot = Time.time;
			}
		}

		lasstShoot = shoot;
	}

	public int getAmmo()
	{
		return ammo;
	}

	public void ProxyShoot(bool shoot){		
	}
}
