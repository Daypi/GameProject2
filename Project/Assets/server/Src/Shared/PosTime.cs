using UnityEngine;
using System.Collections;

public class PosTime   {
	public Vector3 position;
	public float time;
	public Inputstruct commande;
	public double ServerTime;

	public PosTime(Vector3 _pos, float _time, Inputstruct cmd, double _servertime)
	{
		ServerTime = _servertime;
		position = _pos;
		time = _time;
		commande = cmd;
	}

}
