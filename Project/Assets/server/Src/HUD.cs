using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {


	private bool useMaster;
	
	private bool isRequestingHostList ;
	private bool refreshHosts = true;
	private HostData[] hosts;
	
	private string theGame = "Collect in Space 0.0.1";
	
	private int port = 25000;
	private int maxplayers = 5;
	private string serverName  = "CIS Game 1";
	private string ip  = "localhost";
	private int conport = 25000;
	
	public static bool localPlay = false; //Set to true when local game starts


	//GUI controls
	private bool showDediPanel ;
	private bool showClientPanel ;
	private bool showMainPanel  = true;


	void OnGUI() {
		if (showDediPanel) {
			displayDediSettings();
			return; //make sure nothing else renders
		}
		if (showClientPanel) {
			displayClientSettings();
		}
		if (showMainPanel) {
			displayMainPanel();
		}	
	}

	void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("Could not connect to server: "+ error);
	}

	void OnServerInitialized() {
		Debug.Log("Server initialized and ready - loading " + Netman.levelName);
		Application.LoadLevel(Netman.levelName);
		Debug.Log("Connections: " + Network.connections.Length);
	}

	private void displayDediSettings() {
		
		//Port Area
		GUILayout.BeginArea(new Rect(10, 10, 100, 50));
		GUILayout.Label("Port: ");
		port = System.Int32.Parse(GUI.TextField(new Rect(0, 20, 50, 21), port + ""));
		GUILayout.EndArea();
		
		
		//Max Players
		GUILayout.BeginArea(new Rect(65, 10, 100, 50));
		GUILayout.Label("Max. Players: ");
		maxplayers = System.Int32.Parse(GUI.TextField(new Rect(0, 20, 50, 21), maxplayers+""));
		GUILayout.EndArea();
		
		//Game Name
		GUILayout.BeginArea(new Rect(155, 10, 200, 50));
		GUILayout.Label("Game Name: ");
		serverName = GUI.TextField(new Rect(0, 20, 150, 21), serverName);
		GUILayout.EndArea();
		
		Netman.levelName = "CastleWorld";
		
		useMaster = GUI.Toggle(new Rect(195, 60, 130, 19), useMaster, "Use Master Server");
		
		if (GUI.Button(new Rect(10,60,180,25), "Start a Dedicated Server")) {
			Network.InitializeServer(maxplayers, port, !Network.HavePublicAddress());
			if (useMaster) {
				MasterServer.RegisterHost(theGame, serverName);
			}
		}
		
		if (GUI.Button(new Rect(10, 90, 100, 25), "Main Menu")) {
			showMainPanel = true;
			showClientPanel = false;
			showDediPanel = false;
		}
	}

	private void displayClientSettings() {
		//Local Game
		
		GUILayout.BeginArea(new Rect(10, 10, 100, 50));
		GUILayout.Label("Direct connect");
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(10, 30, 100, 50));
		GUILayout.Label("IP or Host name: ");
		ip = GUI.TextField(new Rect(0, 20, 80, 21), ip);
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(120, 30, 100, 50));
		GUILayout.Label("Game Port: ");
		conport = System.Int32.Parse(GUI.TextField(new Rect(0, 20, 80, 21), conport+""));
		GUILayout.EndArea();
		if (GUI.Button(new Rect(10, 80, 100, 24), "Connect")) {
			Debug.Log("Connecting to " + ip + ":" + conport);
			Network.Connect(ip, conport);
		}
		if (GUI.Button(new Rect(10, 110, 100, 24), "Main Menu")) {
			showMainPanel = true;
			showClientPanel = false;
			showDediPanel = false;
		}
		
		GUILayout.BeginArea(new Rect(250, 10, 200, Screen.height - 10));
		GUILayout.BeginArea(new Rect(5, 0, 100, 50));
		GUILayout.Label("Server List: ");
		GUILayout.EndArea();
		
		int lastPosition = 50;
		if (hosts != null) {
			for (int i = 0; i < hosts.Length; i++ ) {
				if (GUI.Button(new Rect(0, 10 + (40 * i), 180, 30), hosts[i].gameName)) {
					Network.Connect(hosts[i]);
					lastPosition = 245 * (40 * i);
				}
			}
		}
		if (GUI.Button(new Rect(0, lastPosition, 95, 25), "Refresh Hosts")) {
			refreshHosts = true;
			isRequestingHostList = false;
		}
		GUILayout.EndArea();
	}

	private void displayMainPanel() {
		GUILayout.BeginArea(new  Rect(10, 10, 200, 80));
		GUILayout.Label("Welcome to Collect in Space\nTo host a local game start\none instance of the game\nas server, then start another\nto connect.");
		GUILayout.EndArea();
		
		if (GUI.Button(new  Rect(210, 10, 100, 25), "Start a Server")) {
			showMainPanel = false;
			showClientPanel = false;
			showDediPanel = true;
		}
		
		if (GUI.Button(new  Rect(210, 50, 150, 25), "Connect to a Server ...")) {
			showMainPanel = false;
			showClientPanel = true;
			showDediPanel = false;
		}
	}

	// Use this for initialization
	void Start () {
		Netman.levelName = "CastleWorld";
	}
	
	// Update is called once per frame
	void Update () {
		if ((refreshHosts && !isRequestingHostList)) {
			MasterServer.RequestHostList(theGame);
			refreshHosts = false;
			isRequestingHostList = true;
		}
		if (isRequestingHostList) {
			if (MasterServer.PollHostList().Length > 0) {
				hosts = MasterServer.PollHostList();
				isRequestingHostList = false;
			}
		}
	}
}
