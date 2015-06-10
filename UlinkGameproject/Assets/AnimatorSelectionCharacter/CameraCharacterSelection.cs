using UnityEngine;
using System.Collections;

public class CameraCharacterSelection : MonoBehaviour {

	int currplayer;
	public string levelname = "";
	// Use this for initialization
	void Start () {
		currplayer = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI() {
		Animation anim = this.GetComponent<Animation> ();
		if (GUI.Button (new Rect (Screen.width * (75f / 100f), Screen.height * (50f / 100f), Screen.width * (25f / 100f), Screen.height * (15f / 100f)), "Next Character")) {
			anim ["Camera0"].speed = 1;
			anim ["Camera1"].speed = 1;
			anim ["Camera2"].speed = 1;
			anim ["Camera3"].speed = 1;
			anim ["Camera4"].speed = 1;
			anim ["Camera5"].speed = 1;
			switch (currplayer) {
			case (0):
				anim.Play ("Camera0");
				break;
			case (1):
				anim.Play ("Camera1");
				break;
			case (2):
				anim.Play ("Camera2");
				break;
			case (3):
				anim.Play ("Camera3");
				break;
			case (4):
				anim.Play ("Camera4");
				break;
			case (5):
				anim.Play ("Camera5");
				break;
			}
			currplayer ++;
			if (currplayer == 6)
				currplayer = 0;

		}
		if (GUI.Button (new Rect (Screen.width * (0f / 100f), Screen.height * (50f / 100f), Screen.width * (25f / 100f), Screen.height * (15f / 100f)), "Previous Character")) {
		
			anim ["Camera0"].speed = -1;
			anim ["Camera0"].time = anim ["Camera0"].length;
			anim ["Camera1"].speed = -1;
			anim ["Camera1"].time = anim ["Camera1"].length;
			anim ["Camera2"].speed = -1;
			anim ["Camera2"].time = anim ["Camera2"].length;
			anim ["Camera3"].speed = -1;
			anim ["Camera3"].time = anim ["Camera3"].length;
			anim ["Camera4"].speed = -1;
			anim ["Camera4"].time = anim ["Camera4"].length;
			anim ["Camera5"].speed = -1;
			anim ["Camera5"].time = anim ["Camera5"].length;
			switch (currplayer - 1) {
			case (-1):
				anim.Play ("Camera5");
				break;
			case (0):
				anim.Play ("Camera0");
				break;
			case (1):
				anim.Play ("Camera1");
				break;
			case (2):
				anim.Play ("Camera2");
				break;
			case (3):
				anim.Play ("Camera3");
				break;
			case (4):
				anim.Play ("Camera4");
				break;
			case (5):
				anim.Play ("Camera5");
				break;
			}
			currplayer --;
			if (currplayer == -1)
				currplayer = 5;
		}
		if (GUI.Button (new Rect (Screen.width * (40f / 100f), Screen.height * (75f / 100f), Screen.width * (25f / 100f), Screen.height * (15f / 100f)), "Lock Selection")) {
			uLink.NetworkView.Get (this).RPC ("Lock", uLink.RPCMode.Server, currplayer);
			Application.LoadLevel(((StartClient)(FindObjectOfType(typeof(StartClient)))).levelname + "Client");
		}
	}

}
