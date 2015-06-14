﻿using UnityEngine;
using System.Collections;

public class MachinGun : Iweapon {
	private double fireDelay = 0.1;
	private float timeSinceLastShoot;
	private int initialammo = 50;
	private int ammo = 60;
	private int damage = -6;
	private GameObject Owner;
	private GameObject Particule;
	private Rewinder rewinder;
	bool isreaload = false;
	bool lasstShoot = false;
	float reloadTime = 1.535f;
	float timeSinceReload;
	float distance = 20.0f;
	public MachinGun(GameObject _owner, GameObject _particule, Rewinder _rewinder){ Owner = _owner; Particule = _particule; rewinder = _rewinder;}
	public void ServerShoot(Vector3 target, Vector3 origin, double time, bool shoot){
		if (isreaload == true) {
			if (Time.time > reloadTime + timeSinceReload)
			{
				isreaload = false;
				ammo = initialammo;
			}
			return;
		}
		if (shoot == true) {
			if (Time.time > fireDelay + timeSinceLastShoot && ammo != 0) {
				timeSinceLastShoot = Time.time;
				Vector3 direction = (target - origin).normalized;
				Debug.DrawRay (origin, direction, Color.red, 5.0f);
				RaycastHit hit;
				if (rewinder.Raycast (origin, direction, out hit, time, distance)) {
					Collider TargetHit = hit.collider;
					Debug.Log (TargetHit);
					if (TargetHit.tag == "Ghostcollider")
						TargetHit.GetComponentInParent<ServerPLayer>().Life(damage, this.Owner.GetComponent<ServerPLayer>().PlayerState.nickname, "MachineGun", this.Owner.GetComponent<ServerPLayer>().PlayerState);
				}
				ammo--;
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
		if (shoot == true && ammo != 0) {
			if (Time.time > fireDelay + timeSinceLastShoot) {  //&& ammo != 0)
				Owner.GetComponent<WeaponManager> ().instantiate (Particule);
				timeSinceLastShoot = Time.time;
				ammo--;
			}
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
