using UnityEngine;
using System.Collections;

public class Natural_Death : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (Network.isServer)
			if (other.tag == "Player")
				other.GetComponent<PlayerManager>().takeDamage(100);
	}
}