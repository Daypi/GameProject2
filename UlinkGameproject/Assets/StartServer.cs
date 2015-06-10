// (c)2011 MuchDifferent. All Rights Reserved.

using System;
using UnityEngine;
using uLink;
using System.Collections.Generic;

/// <summary>
/// A script example that can be used to start a very simple Unity server 
/// listening for uLink connection attempts from clients.
/// </summary>
/// <remarks>
/// The server is listening for UDP traffic on one port number. Default value is 7100.
/// The port number can be changed to whatever port number you like.
/// Another imporant property is targetFrameRate. This value dictates 
/// how many times per second the server reads incoming network traffic 
/// and sends outgoing traffic. It also dictates the actual frame rate for
/// the server (sometimes called tick rate). Read more about tick rate in
/// the Server Operations chapter in the uLink manual.
/// The property called registerHost dictates if this game server should
/// try to register iteself in a uLink Master Server. Read the Master Server & Proxy
/// manual chapter for more info.
/// </remarks>
[AddComponentMenu("uLink Utilities/Simple Server")]
public class StartServer : uLink.MonoBehaviour
{

	private bool isGameStart = false;
	private double elapsedTime = 0;
	public List<string> Names;
	void Update()
	{
		if (isGameStart) {
			CurrentGameTimer -= Time.deltaTime;
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 1.0)
			{
				this.networkView.RPC("UpdateTimer", uLink.RPCMode.Others, CurrentGameTimer);
				elapsedTime = 0.0;
			}
			if (CurrentGameTimer <= 0.0)
			{
				this.networkView.RPC("GameEnding", uLink.RPCMode.Others);
				foreach (LocalPLayerData pl in Players) {
					pl.playerLockSelection = false;
					pl.playerIsReady = false;
				}
				isGameStart = false;
				CurrentGameTimer = GameTimer;
				Application.LoadLevel ("SelectCharacterServerInter");
				int level = UnityEngine.Random.Range (0, 3);
				LevelName = Names [level];
			}
		}
	}

	[Serializable]
	public class InstantiateOnConnected
	{
		public Vector3 startPosition = new Vector3(-22.0f, 100.0f, 0);
		public Vector3 startRotation = new Vector3(0, 0, 0);

		public GameObject ownerPrefabStart;
		public GameObject proxyPrefabStart;
		public GameObject serverPrefabStart;
		public GameObject ownerPrefab;
		public GameObject ownerPrefab1;
		public GameObject ownerPrefab2;
		public GameObject ownerPrefab3;
		public GameObject ownerPrefab4;
		public GameObject ownerPrefab5;
		public GameObject proxyPrefab;
		public GameObject proxyPrefab1;
		public GameObject proxyPrefab2;
		public GameObject proxyPrefab3;
		public GameObject proxyPrefab4;
		public GameObject proxyPrefab5;
		public GameObject serverPrefab;
		public GameObject serverPrefab1;
		public GameObject serverPrefab2;
		public GameObject serverPrefab3;
		public GameObject serverPrefab4;
		public GameObject serverPrefab5;

		public bool appendLoginData = false;

		public void Instantiate(uLink.NetworkPlayer player, int mesh)
		{
				Quaternion rotation = Quaternion.Euler(startRotation);
				GameObject[] respawns = GameObject.FindGameObjectsWithTag("Respawn");
				int selectedId = UnityEngine.Random.Range(0, respawns.Length);
				startPosition = respawns [selectedId].transform.position;
				switch (mesh)
			{
			case 0:
				uLink.Network.Instantiate(player, proxyPrefab, ownerPrefab, serverPrefab, startPosition, rotation, 0, player.loginData);
				break;
			case 1 : 
				uLink.Network.Instantiate(player, proxyPrefab1, ownerPrefab1, serverPrefab1, startPosition, rotation, 0, player.loginData);
				break;
			case 2 : 
				uLink.Network.Instantiate(player, proxyPrefab2, ownerPrefab2, serverPrefab2, startPosition, rotation, 0, player.loginData);
				break;
			case 3 : 
				uLink.Network.Instantiate(player, proxyPrefab3, ownerPrefab3, serverPrefab3, startPosition, rotation, 0, player.loginData);
				break;
			case 4 : 
				uLink.Network.Instantiate(player, proxyPrefab4, ownerPrefab4, serverPrefab4, startPosition, rotation, 0, player.loginData);
				break;
			case 5 : 
				uLink.Network.Instantiate(player, proxyPrefab5, ownerPrefab5, serverPrefab5, startPosition, rotation, 0, player.loginData);
				break;
			}

		}
		public void InstantiateStart(uLink.NetworkPlayer player)
		{
			if (ownerPrefabStart != null && proxyPrefabStart != null && serverPrefabStart != null)
			{
				Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, 448.1682f, 0f));
				Vector3 Position = new Vector3(-30.63f, 1.73f, 32.94f);

				uLink.Network.Instantiate(player, proxyPrefabStart, ownerPrefabStart, serverPrefabStart, Position, rotation, 0, player.loginData);
			}
		}
	}
	
	public int port = 7100;
	public int maxConnections = 2;
	
	public bool cleanupAfterPlayers = true;
	
	public bool registerHost = false;

	public int targetFrameRate = 60;
	public string LevelName = "CastleWorldServer";
	public bool dontDestroyOnLoad = false;
	private List<LocalPLayerData> Players = new List<LocalPLayerData>();
	public double CurrentGameTimer;
	public double GameTimer = 300;

	public InstantiateOnConnected instantiateOnConnected = new InstantiateOnConnected();
	
	void Start()
	{
		Application.targetFrameRate = targetFrameRate;

		if (dontDestroyOnLoad) DontDestroyOnLoad(this);
	
		uLink.Network.InitializeServer(maxConnections, port);
		CurrentGameTimer = GameTimer;
		int level = UnityEngine.Random.Range (0, 2);
		LevelName = Names [level];
	}

	void uLink_OnServerInitialized()
	{
		Debug.Log("Server successfully started on port " + uLink.Network.listenPort);

		if (registerHost) uLink.MasterServer.RegisterHost("Satang", "Satang1" );
	}

	void uLink_OnPlayerDisconnected(uLink.NetworkPlayer player)
	{
		if (cleanupAfterPlayers)
		{
			uLink.Network.DestroyPlayerObjects(player);
			uLink.Network.RemoveRPCs(player);
			
			// this is not really necessery unless you are removing NetworkViews without calling uLink.Network.Destroy
			uLink.Network.RemoveInstantiates(player);
			foreach (LocalPLayerData pl in Players) {
				if (pl.Player == player)
					Players.Remove(pl);
			}
		}
	}

	void uLink_OnPlayerConnected(uLink.NetworkPlayer player)
	{
		this.networkView.RPC ("LevelName", uLink.RPCMode.Others, LevelName);
		instantiateOnConnected.InstantiateStart(player);
		String Name = (string)player.loginData.Read<String>();
		Players.Add(new LocalPLayerData(player, name));
		Debug.Log (name);
	}

	[RPC]
	void ReadyInter(uLink.NetworkMessageInfo info)
	{
		foreach (LocalPLayerData pl in Players) {
			if (pl.Player == info.sender)
				pl.playerIsReadyInter = true;
		}
		instantiateOnConnected.InstantiateStart(info.sender);
		this.networkView.RPC ("LevelName", uLink.RPCMode.Others, LevelName);
	}

	[RPC]
	void Ready(uLink.NetworkMessageInfo info)
	{
		foreach (LocalPLayerData pl in Players) {
			if (pl.Player == info.sender)
				pl.playerIsReady = true;
		}
		foreach (LocalPLayerData pl in Players) {
			if (pl.playerIsReady == false)
				return;
		}
		if (Players.Count == maxConnections) {
			foreach (LocalPLayerData pl in Players) {
				instantiateOnConnected.Instantiate (pl.Player, pl.mesh);
				isGameStart = true;
			}
		}

	}

	[RPC]
	void Lock(int currplayer, uLink.NetworkMessageInfo info)
	{
		info.sender.loginData.Write<int> (currplayer);
		foreach (LocalPLayerData pl in Players) {
			if (pl.Player == info.sender)
			{
				pl.playerLockSelection = true;
				pl.mesh = currplayer;
			}
		}
		if (Players.Count == maxConnections) {
			foreach (LocalPLayerData pl in Players) {
				if (pl.playerLockSelection == false)
					return;
			}
			Application.LoadLevel (LevelName + "Server");
		}
	}
}
