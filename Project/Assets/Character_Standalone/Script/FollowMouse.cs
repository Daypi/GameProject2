using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {
	public GameObject mesh;
	private bool isTheServer = false;
	public bool imowner = false;
	// Use this for initialization
	void Start () {
		if (Network.isServer == true)
			isTheServer = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isTheServer == false)
		{
			Vector3 pos = Input.mousePosition;
			pos.z = transform.position.z - Camera.main.transform.position.z;
			transform.position = Camera.main.ScreenToWorldPoint(pos);
			if (transform.localPosition.x < 0)
			{
				mesh.transform.localRotation = Quaternion.Euler(0,-90,0);
				transform.parent.GetComponent<PlayerInfo>().Facing = -1;
			}
			else
			{
				transform.parent.GetComponent<PlayerInfo>().Facing = 1;
				mesh.transform.localRotation = Quaternion.Euler(0,90,0);
			}
			GetComponent<NetworkView>().RPC("UpdatePosition", RPCMode.Server, this.transform.position, transform.parent.GetComponent<PlayerInfo>().Facing);
		}
	}

	[RPC]
	void UpdatePosition(Vector3 _position, int facing)
	{
		if (isTheServer == true) 
		{
			this.transform.position = _position;
			if (transform.localPosition.x < 0)
			{
				mesh.transform.localRotation = Quaternion.Euler(0,-90,0);
				transform.parent.GetComponent<PlayerInfo>().Facing = -1;
			}
			else
			{
				transform.parent.GetComponent<PlayerInfo>().Facing = 1;
				mesh.transform.localRotation = Quaternion.Euler(0,90,0);
			}

		}
	}

	void LateUpdate()
	{
	}
}
