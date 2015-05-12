using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

	public AnimatorState State = AnimatorState.Idle;
	public float movement;
	public int Facing;
	public float Hp = 100f;
	public bool dead = false;
	public GameObject mesh;
	// Use this for initialization
	void Start () {
		Hp = 100f;
	}


	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		float _Hp = 100;
		int _State = (int)AnimatorState.Idle;
		float _movement = 0f;
		int _Facing = 0;
		bool _dead = false;
		if (stream.isWriting) {
			_Hp = Hp;
			_State = (int)State;
			_movement = movement;
			_Facing = Facing;
			_dead = dead;
			stream.Serialize(ref _Hp);
			stream.Serialize(ref _State);
			stream.Serialize(ref _movement);
			stream.Serialize(ref _Facing);
			stream.Serialize(ref _dead);
		} else {
			stream.Serialize(ref _Hp);
			stream.Serialize(ref _State);
			stream.Serialize(ref _movement);
			stream.Serialize(ref _Facing);
			stream.Serialize(ref _dead);
			Hp = _Hp;
			State = (AnimatorState) _State;
			movement = _movement;
			Facing = _Facing;
			dead = _dead;
		}
	}


	// Update is called once per frame
	void Update () {
	
		if (Facing == 1)
			mesh.transform.localRotation = Quaternion.Euler(0,90,0);
		else
			mesh.transform.localRotation = Quaternion.Euler(0,-90,0);

	}
}
