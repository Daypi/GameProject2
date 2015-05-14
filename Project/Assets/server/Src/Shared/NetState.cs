using UnityEngine;
using System.Collections;

public class NetState {
	
	public double timestamp  ; //The time this state occured on the network
	public Vector3 pos  ; //Position of the attached object at that time
	public Quaternion rot  ; //Rotation at that time
	
	public NetState() {
		timestamp = 0.0f;
		pos = Vector3.zero;
		rot = Quaternion.identity;
	}
	
	public NetState(double time , Vector3 pos  , Quaternion rot  ) {
		timestamp = time;
		this.pos = pos;
		this.rot = rot;
	}
}
