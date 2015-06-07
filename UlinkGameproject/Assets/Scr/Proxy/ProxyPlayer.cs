using UnityEngine;
using System.Collections;

public class ProxyPlayer : MonoBehaviour {
	public LayerMask mask = -1;
	// Use this for initialization
	void Start () {
		Physics.IgnoreLayerCollision(8, 8);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.FindChild ("Aim").transform.localPosition.x < 0) {
			this.transform.FindChild("Skeleton").transform.localRotation = Quaternion.Euler(0,-90,0);
		} else
		{
			this.transform.FindChild("Skeleton").transform.localRotation = Quaternion.Euler(0,90,0);
		}
	}
}
