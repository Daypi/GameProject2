using UnityEngine;

public interface Iweapon  {

	GameObject owner { get;  set; }
	void shoot(float angle);

}
