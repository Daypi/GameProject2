using UnityEngine;
using System.Collections;

public class RewinderStruct {
	public RewinderStruct (Vector3 _position, double _NetworkTime)
	{
		position = _position;
		NetworkTime = _NetworkTime;
	}
	public Vector3 position;
	public double NetworkTime;
}
