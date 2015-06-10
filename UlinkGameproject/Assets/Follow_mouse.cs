using UnityEngine;
using System.Collections;

public class Follow_mouse : MonoBehaviour {

	public float angle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 10;
		Vector3 mouse = Camera.main.GetComponent<Camera>().ScreenToWorldPoint  (mousePos);
		this.angle = (GetAngle (this.transform.position, mouse));
		FollowMousse ();
	}

	void FollowMousse()
	{
		float localangle = angle;
		float frame; //= (5f * localangle) / 90 ;
		if (localangle < 90 && localangle > -90)
		{
			//        transform.localEulerAngles = new Vector3(0,90,0);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0) ,  0.2f);
		}
		else
		{
			//    transform.localEulerAngles = new Vector3(0,-90,0);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0) ,  0.2f);
			if (localangle > -180 && localangle < -90)
			{
				localangle = localangle * -1f;
				localangle = (localangle - 180f);
			}
			else
				localangle = localangle - 90f;
		}
			//GetComponent<Animation>()["UnrealAim"].time = ((frame + 15f)/30f);
//		GetComponent<Animation>()["UnrealAim"].speed = 0; 
		
	}

	void LateUpdate() {
		float localangle = angle;
		float frame;
		if (angle < 0)
			frame = (-5f * angle) / 90 ;
		else
			frame = (5f * angle) / 90 ;
		if (localangle >= 0f && localangle <= 90f)
			GetComponent<Animator>().Play("Unreal Take", -1, (frame + 5f) / 30f);
		//GetComponent<Animation>() ["UnrealAim"].time = (frame + 5f) / 30f;
		if (localangle <= 0f && localangle >= -90f)
			GetComponent<Animator> ().Play ("Unreal Take", -1, (frame + 15f) / 30f);
	}


	float GetAngle(Vector3 from, Vector3 to)
	{
		Vector2 From = new Vector2(from.x, from.y);
		Vector2 To = new Vector2(to.x, to.y) ;
		float angleBetween = Mathf.Atan2(To.y - From.y, To.x - From.x) * 180 / Mathf.PI;
		return angleBetween;
		
	}
}
