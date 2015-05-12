using UnityEngine;
using System.Collections;

public class Animation_Controller : MonoBehaviour {
	PlayerInfo playerinfo;
	Animator animation;
	// Use this for initialization
	void Start () {
		playerinfo = this.transform.parent.GetComponent<PlayerInfo> ();
		animation = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		switch(playerinfo.State)
		{
		case AnimatorState.JumpStart:
			animation.SetBool("Jump" , true);
			break;
		case AnimatorState.Fall:
			animation.SetBool("Fall" , true);
			animation.SetBool("Jump" , false);
			break;
		case AnimatorState.Land:
			animation.SetBool("Land" , true);
			animation.SetBool("Fall" , false);
			break;
		case AnimatorState.StopLand:
			animation.SetBool("Land" , false);
			break;
		}
		animation.SetFloat ("Movement", playerinfo.movement * playerinfo.Facing);

	}
}
