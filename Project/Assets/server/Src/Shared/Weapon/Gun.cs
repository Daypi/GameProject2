using UnityEngine;
using System.Collections;

public class Gun:  Iweapon  {
	private float fireDelay;
	private float timeSinceLastShoot;
	public GameObject owner { get;  set; }
	private int ammo = 10;
	private int damage = 20;
	public GameObject particle;

	public Gun(GameObject _owner)
	{
		owner = _owner;
		fireDelay = 0.0f;
	}

	public int Ammo
	{
		get {return ammo;}
		set {ammo = value;}
	}

	public void C_shoot(GameObject target, Vector3 child){

	}

	public void shoot(GameObject tar, Vector3 child){
		if (Time.time > fireDelay + timeSinceLastShoot) //&& ammo != 0)
		{
			Vector3 direction = tar.transform.position - child;
			Debug.DrawRay (child, direction * 4, Color.red, 5.0f);
			RaycastHit hit;
			Vector3 bones = child;//GameObject.Find ("ak/Cannon");
			if (Physics.Raycast(bones, direction, out hit))
			{
				Collider target = hit.collider; // What did I hit?
				float distance = hit.distance; // How far out?
				Vector3 location = hit.point; // Where did I make impact?
				GameObject targetGameObject = hit.collider.gameObject; // What's the GameObject?
				Debug.Log(target);
                if (target.tag == "Player")
                    target.GetComponent<PlayerManager>().takeDamage(damage);
			}
			//GameObject projectile =(GameObject)GameObject.Instantiate(projectilePrefab, bones.transform.position, Quaternion.Euler(new Vector3(angle * -1,90,0)));
			timeSinceLastShoot = Time.time;
			ammo--;
		}
	}
}
