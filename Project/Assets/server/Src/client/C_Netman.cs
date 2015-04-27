using UnityEngine;
using System.Collections;

public class C_Netman : MonoBehaviour {

	void  OnConnectedToServer() {
		Debug.Log("Disabling message queue!");
		Network.isMessageQueueRunning = false;
		Application.LoadLevel(Netman.levelName);
	}

	void OnLevelWasLoaded(int level) {
		Debug.Log (level);
		if (level != 0 && Network.isClient) { //0 is my menu scene so ignore that.
			Network.isMessageQueueRunning = true;
			Debug.Log("Level was loaded, requesting spawn");
			Debug.Log("Re-enabling message queue!");
			//Request a player instance form the server
			GetComponent<NetworkView>().RPC("requestSpawn", RPCMode.Server, Network.player);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
