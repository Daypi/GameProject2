using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
			Vector3 pos = Input.mousePosition;
			pos.z = transform.position.z - Camera.main.transform.position.z;
			transform.position = Camera.main.ScreenToWorldPoint(pos);
			if (transform.localPosition.x < 0)
			{
			//	transform.parent.GetComponent<PlayerInfo>().Facing = -1;
			}
			else
			{
	//			transform.parent.GetComponent<PlayerInfo>().Facing = 1;
			}
		//	GetComponent<NetworkView>().RPC("UpdatePosition", RPCMode.Server, this.transform.position, transform.parent.GetComponent<PlayerInfo>().Facing);
	}

	void LateUpdate()
	{
	}
}
