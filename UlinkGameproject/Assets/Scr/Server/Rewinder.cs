using UnityEngine;
using System.Collections;

public class Rewinder : MonoBehaviour {

	public CircularBuffer<RewinderStruct> Positions = new CircularBuffer<RewinderStruct> (100);
	public GameObject GhostCollider;
	Rewinder[] rewinders;
	// Use this for initialization
	void Start () {
		rewinders = FindObjectsOfType(typeof(Rewinder)) as Rewinder[];
	}
	
	public void AddRewinderstruct(RewinderStruct rewind)
	{
		Positions.Add (rewind);
	}

	public bool Raycast(Vector3 position, Vector3 Direction, out RaycastHit hit, double time)
	{
		Vector3 pos = Vector3.up;
		foreach (Rewinder rewind in rewinders) {
			foreach (RewinderStruct RS in rewind.Positions) {
				if (RS.NetworkTime >= time && RS.NetworkTime <= (RS.NetworkTime + 0.02))
					pos = RS.position;
				if (rewind.GhostCollider != this.GhostCollider)
					rewind.GhostCollider.transform.position = pos;
			}
		}
		bool ret = Physics.Raycast (position, Direction, out hit, 8);
		foreach (Rewinder rewind in rewinders) {
			foreach (RewinderStruct RS in rewind.Positions) {
				if (RS.NetworkTime >= time && RS.NetworkTime <= (RS.NetworkTime + 0.02))
					pos = RS.position;
				rewind.GhostCollider.transform.position = new Vector3(0, 0, -100);
			}
		}
		return ret;
	}

	public bool Raycast(Ray ray, out RaycastHit hit, double time)
	{
		Vector3 pos = Vector3.up;
		foreach (Rewinder rewind in rewinders) {
			foreach (RewinderStruct RS in rewind.Positions) {
				if (RS.NetworkTime >= time && RS.NetworkTime <= (RS.NetworkTime + 0.02))
					pos = RS.position;
				if (rewind.GhostCollider == this.GhostCollider)
					rewind.GhostCollider.transform.position = pos;
			}
		}
		Debug.DrawLine (ray.origin, ray.direction);
		bool ret = Physics.Raycast (ray, out hit);
		foreach (Rewinder rewind in rewinders) {
			foreach (RewinderStruct RS in rewind.Positions) {
				if (RS.NetworkTime >= time && RS.NetworkTime <= (RS.NetworkTime + 0.02))
					pos = RS.position;
				rewind.GhostCollider.transform.position = new Vector3(0, 0, -100);
			}
		}
		return ret;
	}

	/*
		public Vector3 ReturnPosGizmos (double time)
	{
		Vector3 pos = Vector3.up;
		foreach (RewinderStruct RS in Positions) {
			if (RS.NetworkTime >= time && RS.NetworkTime <= (RS.NetworkTime + 0.02))
				pos = RS.position;
		}
		return pos;
	}
		
		[DrawGizmo (GizmoType.Selected | GizmoType.Active)]
		static void DrawGizmoForMyScript (Rewinder scr, GizmoType gizmoType) {
			Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(scr.ReturnPosGizmos(Time.fixedTime - 0.9), 1);
		}
*/
/*	void Update()
	{
		if (Input.GetMouseButton(0)){ // if you press Left Mouse Button -> GetMouseButtonDown(0) 
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // it sends a ray from Camera.main
			Vector3 dir = Vector3.forward;
			if (Raycast(ray, out hit, (Time.fixedTime - 0.9))){ // if it hits something         
				Debug.Log(hit.transform.name);
			}
		}
	}
	// Update is called once per frame
	void FixedUpdate () {
		this.AddRewinderstruct(new RewinderStruct(this.transform.position, Time.fixedTime));
	}*/
}
