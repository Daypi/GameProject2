using UnityEngine;
using System.Collections;

public class LocalPLayerData {

	public uLink.NetworkPlayer Player;
	public string NickName = "Default";
	public int mesh;
	public bool playerLockSelection = false;
	public bool playerIsReady = false;
	public LocalPLayerData(uLink.NetworkPlayer _player, string _nickname)
	{
		Player = _player;
		NickName = _nickname;
	}

}
