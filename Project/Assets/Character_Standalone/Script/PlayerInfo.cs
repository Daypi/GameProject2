using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

	public AnimatorState State = AnimatorState.Idle;
	public float movement;
	public int Facing;
	public int Hp = 100;
	public bool dead = false;
	// Use this for initialization
	void Start () {
	
	}


	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		int _Hp = 100;
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
			stream.Serialize(ref Hp);
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
	
	}
}
