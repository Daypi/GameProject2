using UnityEngine;

public interface Iweapon  {

	GameObject owner { get;  set; }
	void shoot(GameObject target, Vector3 child);
	void C_shoot(GameObject target, Vector3 child);
}
