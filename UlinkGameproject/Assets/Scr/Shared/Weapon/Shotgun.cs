using UnityEngine;
using System.Collections;

public class ShotGun : Iweapon {
	private float fireDelay = 0.1f;
	private float timeSinceLastShoot;
	private int initialammo = 6;
	private int ammo = 6;
	private int damage = -8;
	private GameObject Owner;
	private GameObject Particule;
	private Rewinder rewinder;
	private float spreadFactor = 1.0f;
	bool isreaload = false;
	bool lasstShoot = false;
	float reloadTime = 0.68f;
	float timeSinceReload;
	float distance = 20.0f;
	public ShotGun(GameObject _owner, GameObject _particule, Rewinder _rewinder){ Owner = _owner; Particule = _particule; rewinder = _rewinder;}
	public void ServerShoot(Vector3 target, Vector3 origin, double time, bool shoot){
		if (isreaload == true) {
			if (Time.time > reloadTime + timeSinceReload)
			{
				isreaload = false;
				ammo += 1;
			}
			return;
		}
		if (shoot == true && lasstShoot == false) {
			if (Time.time > fireDelay + timeSinceLastShoot && ammo != 0) {
				timeSinceLastShoot = Time.time;
				for (int i = 0; i <= 6; i++)
				{
				Vector3 direction = (target - origin).normalized;
				direction.x += Random.Range(-spreadFactor, spreadFactor);
				direction.y += Random.Range(-spreadFactor, spreadFactor);
				Debug.DrawRay (origin, direction, Color.red, 5.0f);
				RaycastHit hit;
					if (rewinder.Raycast (origin, direction, out hit, time, distance)) {
					Collider TargetHit = hit.collider;
					Debug.Log (TargetHit);
					if (TargetHit.tag == "Ghostcollider")
						TargetHit.GetComponentInParent<ServerPLayer>().Life(damage, this.Owner.GetComponent<ServerPLayer>().PlayerState.nickname, "Shotgun", this.Owner.GetComponent<ServerPLayer>().PlayerState);
				}
				}
				ammo--;
				Owner.GetComponent<ServerPLayer>().SendProxyShoot();
			}
		}
		lasstShoot = shoot;
	}

	public void ServerReload()
	{
		if (isreaload != true && ammo < initialammo) {
			isreaload = true;
			timeSinceReload = Time.time;
		}
	}

	public void ClientReload()
	{
		if (isreaload != true && ammo < initialammo) {
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
				ammo += 1;
			}
			return;
		}
		if (shoot == true && lasstShoot == false && ammo != 0) {
			if (Time.time > fireDelay + timeSinceLastShoot) {  //&& ammo != 0)
				Owner.GetComponent<WeaponManager> ().instantiate (Particule);
				timeSinceLastShoot = Time.time;
			}
			ammo--;
		}

		lasstShoot = shoot;
	}

	public int getAmmo()
	{
		return ammo;
	}

	public void ProxyShoot(bool shoot){		
			Owner.GetComponent<WeaponManager> ().instantiate (Particule);
	}
}
