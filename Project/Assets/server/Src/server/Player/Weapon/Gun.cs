using UnityEngine;
using System.Collections;

public class Gun:  Iweapon  {
	private float fireDelay;
	private float timeSinceLastShoot;
	public GameObject owner { get;  set; }
	public GameObject projectilePrefab;
	private int ammo = 10;
	private int dammage = 20;

	public Gun(GameObject _owner, GameObject Projectile)
	{
		projectilePrefab = Projectile;
		owner = _owner;
		fireDelay = 0.5f;
	}

	public int Ammo
	{
		get {return ammo;}
		set {ammo = value;}
	}

	public void shoot(float angle){
		if (Time.time > fireDelay + timeSinceLastShoot) //&& ammo != 0)
		{
			Debug.Log ("ici je shoot avec le gun");
			Vector3 vForce = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
			vForce.Normalize ();
			Debug.DrawRay (owner.transform.position, vForce, Color.red, 2.0f);
			RaycastHit hit;
			GameObject bones = owner;//GameObject.Find ("ak/Cannon");
			if (Physics.Raycast(bones.transform.position, vForce, out hit))
			{
				Collider target = hit.collider; // What did I hit?
				float distance = hit.distance; // How far out?
				Vector3 location = hit.point; // Where did I make impact?
				GameObject targetGameObject = hit.collider.gameObject; // What's the GameObject?
				PlayerManager manager = targetGameObject.GetComponent<PlayerManager>();
				manager.Hp = manager.Hp - 10;
				Debug.Log(target);
			}
			//GameObject projectile =(GameObject)GameObject.Instantiate(projectilePrefab, bones.transform.position, Quaternion.Euler(new Vector3(angle * -1,90,0)));
			timeSinceLastShoot = Time.time;
			ammo--;
		}
	}
}
