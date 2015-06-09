using UnityEngine;
using System.Collections;

public interface Iweapon   {
	void ServerShoot(Vector3 target, Vector3 origin, double time, bool shoot);
	void ClientShoot(bool shoot);
	void ProxyShoot(bool shoot);
	void ClientReload();
	void ServerReload();
	void ProxyReload();
}
