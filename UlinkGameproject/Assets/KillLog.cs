using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KillLog : MonoBehaviour {
    public Queue<string> kills;
    float timer = 3.0f;
    float startTimer = 0;
	// Use this for initialization
	void Start () {
        kills = new Queue<string>();
        startTimer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        if (startTimer + timer < Time.time)
        {
            if (kills.Count > 0)
                kills.Dequeue();
            startTimer = Time.time;
        }
            

        string killText = "";

        foreach (string tmp in kills)
	    {
            Debug.Log("tmp dans forich= " + tmp);
            killText += tmp;
	    }
        this.GetComponent<Text>().text = killText;
	}
}
