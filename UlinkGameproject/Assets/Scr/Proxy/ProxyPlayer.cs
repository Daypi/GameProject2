using UnityEngine;
using System.Collections;

public class ProxyPlayer : uLink.MonoBehaviour {
	public LayerMask mask = -1;
	public StructCodec.PlayerStateStruct PlayerState = new StructCodec.PlayerStateStruct();
	// Use this for initialization
	void Start () {
		Physics.IgnoreLayerCollision(8, 8);
	}

	void uLink_OnNetworkInstantiate (uLink.NetworkMessageInfo info ) { 
		System.RuntimeTypeHandle toto = new System.RuntimeTypeHandle();
        PlayerState.nickname = (string)info.networkView.initialData.Read<string>();
	} 

	[RPC]
	public void TestRpc(uLink.NetworkMessageInfo info)
	{
		if (info.networkView.viewID == this.networkView.viewID) {
			object[] tempStorage = new object[2];
			tempStorage [0] = 0.0;
			tempStorage [1] = true;
			SendMessage ("Shoot", tempStorage);
		}
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
