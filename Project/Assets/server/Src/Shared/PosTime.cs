using UnityEngine;
using System.Collections;

public class PosTime   {
	public Vector3 position;
	public float time;
	public Inputstruct commande;

	public PosTime(Vector3 _pos, float _time, Inputstruct cmd)
	{
		position = _pos;
		time = _time;
		commande = cmd;
	}

}
