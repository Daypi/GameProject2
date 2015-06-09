using UnityEngine;
using System.Collections;

public class ClientSelectCharacter : uLink.MonoBehaviour {

	void uLink_OnNetworkInstantiate (uLink.NetworkMessageInfo info ) { 
		string Name = "Uknown";
		System.RuntimeTypeHandle toto = new System.RuntimeTypeHandle();
		Name = (string)info.networkView.initialData.ReadObject(typeof(string).TypeHandle);
		info.networkView.initialData.Write<string> (name);
	} 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
