using UnityEngine;
using System.Collections;

public class Anim : MonoBehaviour {

	private GameObject parent;
	private float angle;
	private C_PlayerManager PLparent;
	private float lastmove = 0;

	void Start () {
		parent = this.transform.parent.gameObject;
		PLparent = 	parent.GetComponent<C_PlayerManager> ();
		GetComponent<Animation>().Play("UnrealAim");
	}


	void Update () {
		angle = PLparent.angle;
		FollowMousse ();
		Jog ();
	}

	void jumpanim()
	{

		GetComponent<Animation>()["Jump"].wrapMode = WrapMode.Once;
		GetComponent<Animation>()["Jump"].layer = 10000;
		GetComponent<Animation>().Play("Jump");
	}

	void Jog()
	{ 
		if (angle < 90 && angle > -90)
		if (Input.GetAxis ("Horizontal") != 0f) {
			if (angle < 90 && angle > -90 && Input.GetAxis ("Horizontal") > 0)
				{
		

						GetComponent<Animation>()["JogForward"].speed = 1;
						GetComponent<Animation> () ["JogForward"].wrapMode = WrapMode.Loop;
						GetComponent<Animation> () ["JogForward"].layer = 500;
						GetComponent<Animation> ().Play ("JogForward");
				}
			else if (angle < 90 && angle > -90 && Input.GetAxis ("Horizontal") < 0)
			{
				
				
				GetComponent<Animation>()["JogBackward"].speed = 1;
				GetComponent<Animation> () ["JogBackward"].wrapMode = WrapMode.Loop;
				GetComponent<Animation> () ["JogBackward"].layer = 500;
				GetComponent<Animation> ().Play ("JogBackward");
			}
			else if ( Input.GetAxis ("Horizontal") < 0)
			{
				
				
				GetComponent<Animation>()["JogForward"].speed = 1;
				GetComponent<Animation> () ["JogForward"].wrapMode = WrapMode.Loop;
				GetComponent<Animation> () ["JogForward"].layer = 500;
				GetComponent<Animation> ().Play ("JogForward");
			}
			else
			{
				
				
				GetComponent<Animation>()["JogBackward"].speed = 1;
				GetComponent<Animation> () ["JogBackward"].wrapMode = WrapMode.Loop;
				GetComponent<Animation> () ["JogBackward"].layer = 500;
				GetComponent<Animation> ().Play ("JogBackward");
			}
		}
		else
		{
			GetComponent<Animation> ()["JogForward"].speed = 0;
			GetComponent<Animation>()["JogBackward"].speed = 0;
		}
	}	
	

	void FollowMousse()
	{
		GetComponent<Animation>().Play("UnrealAim");
		GetComponent<Animation>()["UnrealAim"].layer = 1;
		float localangle = angle;
		float frame; //= (5f * localangle) / 90 ;
		if (localangle < 90 && localangle > -90)
		{
			//		transform.localEulerAngles = new Vector3(0,90,0);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0) ,  0.2f);
		}
		else
		{
			//	transform.localEulerAngles = new Vector3(0,-90,0);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0) ,  0.2f);
			if (localangle > -180 && localangle < -90)
			{
				localangle = localangle * -1f;
				localangle = (localangle - 180f);
			}
			else
				localangle = localangle - 90f;
		}
		if (angle < 0)
			frame = (-5f * angle) / 90 ;
		else
			frame = (5f * angle) / 90 ;
		if (localangle >= 0f && localangle <= 90f)
			GetComponent<Animation>() ["UnrealAim"].time = (frame + 5f) / 30f;
		if (localangle <= 0f && localangle >= -90f )
			GetComponent<Animation>()["UnrealAim"].time = ((frame + 15f)/30f);
		GetComponent<Animation>()["UnrealAim"].speed = 0; 
		
	}
}
