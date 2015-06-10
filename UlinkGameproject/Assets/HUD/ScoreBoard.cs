using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreBoard : MonoBehaviour {
    public string scores;
    public string timeleft = "";
	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
        scores = "";
        scores += "Time Left : " + timeleft;
        scores += "\nName \t   Kills \t Deaths \n";
        ClientPlayer player = FindObjectOfType(typeof(ClientPlayer)) as ClientPlayer;
        if (player)
            scores += player.PlayerState.nickname + "\t\t" + player.PlayerState.NbKill + "\t\t" + player.PlayerState.NbDead + "\n";
        ProxyPlayer[] proxies = FindObjectsOfType(typeof(ProxyPlayer)) as ProxyPlayer[];
        foreach (var proxy in proxies)
        {
            scores += proxy.PlayerState.nickname + "\t\t" + proxy.PlayerState.NbKill + "\t\t" + proxy.PlayerState.NbDead + "\n";
        }
        this.GetComponent<Text>().text = scores;
        this.GetComponentInChildren<Text>().text = scores;
	}
}
