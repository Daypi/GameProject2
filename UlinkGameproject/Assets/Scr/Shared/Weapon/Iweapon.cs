using UnityEngine;
using System.Collections;

public interface Iweapon   {
	void ServerShoot(Vector3 target, Vector3 origin, double time);
	void ClientShoot();
	void ProxyShoot();
}
