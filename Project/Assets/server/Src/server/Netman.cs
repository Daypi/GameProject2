using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Netman : MonoBehaviour {

	public GameObject player; //player object to instantiate
	public static string levelName; //current level name

	private List<C_PlayerManager> playerTracker  = new List<C_PlayerManager>();
	private List<NetworkPlayer> scheduledSpawns  = new List<NetworkPlayer>();
	
	private bool processSpawnRequests = false;


	void OnPlayerConnected(NetworkPlayer player){
		Debug.Log("Spawning prefab for new client");
		scheduledSpawns.Add(player);
		processSpawnRequests = true;
	}

	[RPC]
	void requestSpawn(NetworkPlayer requester) {
		//Called from client to the server to request a new entity
		if (Network.isClient) {
			Debug.LogError("Client tried to spawn itself! Revise logic!");
			return; //Get lost! This is server business
		}
		if (!processSpawnRequests) {
			return; //silently ignore this
		}
		//Process all scheduled players
		foreach (NetworkPlayer spawn in scheduledSpawns) {
			Debug.Log("Checking player " + spawn.guid);
			if (spawn == requester) { //That is the one, lets make him an entity!
				int num  = System.Int32.Parse(spawn + "");
				GameObject handle =  (GameObject)Network.Instantiate(
					player, 
					transform.position,
					Quaternion.Euler(new Vector3(0, 90, 0)),
					1);
				C_PlayerManager sc = handle.GetComponent<C_PlayerManager>();
				if (!sc) {
					Debug.LogError("The prefab has no C_PlayerManager attached!");
				}
				playerTracker.Add(sc);
				//Get the network view of the player and add its owner
				NetworkView netView  = handle.GetComponent<NetworkView>();
				netView.RPC("setOwner", RPCMode.AllBuffered, spawn);
			}
		}
		scheduledSpawns.Remove(requester); //Remove the guy from the list now
		if (scheduledSpawns.Count == 0) {
			Debug.Log("spawns is empty! stopping spawn request processing");
			//If we have no more scheduled spawns, stop trying to process spawn requests
			processSpawnRequests = false;
		}
	}


	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Player " + player.guid + " disconnected.");
		C_PlayerManager found = null;
		foreach (C_PlayerManager man in playerTracker) {
			if (man.getOwner() == player) {
				Network.RemoveRPCs(man.gameObject.GetComponent<NetworkView>().viewID);
				Network.Destroy(man.gameObject);
			}
		}
		if (found) {
			playerTracker.Remove(found);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
