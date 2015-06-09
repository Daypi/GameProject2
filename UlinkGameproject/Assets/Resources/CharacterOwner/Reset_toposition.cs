using UnityEngine;
using System.Collections;

public class Reset_toposition : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localPosition = new Vector3 (0f, 0f, 0f);
	}
}
