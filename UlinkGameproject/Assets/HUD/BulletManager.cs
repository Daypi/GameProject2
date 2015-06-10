using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BulletManager : MonoBehaviour {
    public int currentBullets = 0;
    int maxBullets = 60;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        int i = 1;
	    while (i <= currentBullets)
        {
            GameObject.Find("PlayerHUD/" + i).GetComponent<Image>().enabled = true;
            i++;
        }
        while (i <= maxBullets)
        {
            GameObject.Find("PlayerHUD/" + i).GetComponent<Image>().enabled = false;
            i++;
        }
	}
}
