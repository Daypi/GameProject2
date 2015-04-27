using UnityEngine;
using System.Collections.Generic;

public class Weapon_gestion  {

	private List<Iweapon> weapon;
	int current;
	private GameObject ShootGunpParticle;
	private GameObject GunParticle;
	private GameObject FlamethrowerParticle;

	// Use this for initialization
	public Weapon_gestion (GameObject owner, GameObject _ShootGunpParticle, GameObject _GunParticle, GameObject _FlamethrowerParticle) {
		weapon = new List<Iweapon>();
		weapon.Add (new Gun(owner, _GunParticle));
		current = 0;
		ShootGunpParticle = _ShootGunpParticle;
		GunParticle = _GunParticle;
		FlamethrowerParticle = _FlamethrowerParticle;
	}

	public void shoot(float angle)
	{
		weapon[0].shoot (angle);
	/*	Vector3 vForce = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
		vForce.Normalize ();
		Debug.DrawRay (weapon[0].owner.transform.position, vForce, Color.red, 2.0f);*/
	}

	public void ChangeWeapon()
	{

	}
}
