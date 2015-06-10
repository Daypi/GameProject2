using UnityEngine;
using System.Collections;

public class NaturalDeath : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player")
			other.GetComponent<ServerPLayer> ().Life (-200, "MotherNature", "Injustice", null);
	}
	
}
