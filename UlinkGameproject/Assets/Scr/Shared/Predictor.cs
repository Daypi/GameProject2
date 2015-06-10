using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Predictor : uLink.MonoBehaviour {

	private StructCodec.ResultStruct[] proxyStates = new StructCodec.ResultStruct[20];
	public double interpolationBackTime = 0.2;
	public double extrapolationLimit = 0.5;
	private double serverLastTimestamp = 0;
	private int proxyStateCount;
    public string killmessage = "";
	// Use this for initialization
	void Start () {
	
	}

	void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		serverLastTimestamp = info.rawServerTimestamp;
	}

	// Update is called once per frame
	void Update () {
        //killmessage = "";
        float hp = 100;
        bool dead = false;
        string pname = "Player";
        if (!uLink.Network.isServer)
        {
            if (this.networkView.isOwner)
            {
                hp = this.GetComponent<ClientPlayer>().PlayerState.life;
                dead = this.GetComponent<ClientPlayer>().PlayerState.isdead;
                pname = this.GetComponent<ClientPlayer>().PlayerState.nickname;
            }
            else if (this.networkView.isProxy)
            {
                hp = this.GetComponent<ProxyPlayer>().PlayerState.life;
                dead = this.GetComponent<ProxyPlayer>().PlayerState.isdead;
                pname = this.GetComponent<ProxyPlayer>().PlayerState.nickname;
            }   
            this.GetComponent<HpBar>().scale = hp / 100.0f;
            this.GetComponent<HpBar>().enabled = !dead;
            this.GetComponent<HpBar>().pname = pname;
            
        }
		if (uLink.Network.isAuthoritativeServer && uLink.Network.isServerOrCellServer)
			return;
		if (uLink.Network.isAuthoritativeServer && !networkView.isMine) {
			double interpolationTime = uLink.NetworkTime.rawServerTime - interpolationBackTime;
			if (proxyStates[0].timestamp > interpolationTime)
			{
				// Go through buffer and find correct state to play back
				for (int i = 0; i < proxyStateCount; i++)
				{
					if (proxyStates[i].timestamp <= interpolationTime || i == proxyStateCount - 1)
					{
						// The state one slot newer (<100ms) than the best playback state
						StructCodec.ResultStruct rhs = proxyStates[Mathf.Max(i - 1, 0)];
						// The best playback state (closest to 100 ms old (default time))
						StructCodec.ResultStruct lhs = proxyStates[i];
						
						// Use the time between the two slots to determine if interpolation is necessary
						double length = rhs.timestamp - lhs.timestamp;
						float t = 0.0F;
						// As the time difference gets closer to 100 ms t gets closer to 1 in 
						// which case rhs is only used
						// Example:
						// Time is 10.000, so sampleTime is 9.900 
						// lhs.time is 9.910 rhs.time is 9.980 length is 0.070
						// t is 9.900 - 9.910 / 0.070 = 0.14. So it uses 14% of rhs, 86% of lhs
						if (length > 0.0001)
							t = (float)((interpolationTime - lhs.timestamp) / length);
						
						// if t=0 => lhs is used directly
						if (rhs.position.x > lhs.position.x)
							this.transform.FindChild("Skeleton").GetComponent<Animator>().SetFloat("Movement", 1.0f);
						else if (rhs.position.x < lhs.position.x)
							this.transform.FindChild("Skeleton").GetComponent<Animator>().SetFloat("Movement", -1.0f);
						else
							this.transform.FindChild("Skeleton").GetComponent<Animator>().SetFloat("Movement", 0.0f);
						if (rhs.position.y > (lhs.position.y + 0.000001) && rhs.position != lhs.position)
						{
							Debug.Log (rhs.position.y + ":::::::" + lhs.position.y);
							this.transform.FindChild("Skeleton").GetComponent<Animator>().SetBool("Jump", true);
							this.transform.FindChild("Skeleton").GetComponent<Animator>().SetBool("Fall", false);
							this.transform.FindChild("Skeleton").GetComponent<Animator>().SetBool("Land", false);
						}
						else if (rhs.position.y < lhs.position.y)
						{
							this.transform.FindChild("Skeleton").GetComponent<Animator>().SetBool("Jump", false);
							this.transform.FindChild("Skeleton").GetComponent<Animator>().SetBool("Fall", true);
							this.transform.FindChild("Skeleton").GetComponent<Animator>().SetBool("Land", false);
						}
						else
						{
							this.transform.FindChild("Skeleton").GetComponent<Animator>().SetBool("Jump", false);
							this.transform.FindChild("Skeleton").GetComponent<Animator>().SetBool("Fall", false);
							this.transform.FindChild("Skeleton").GetComponent<Animator>().SetBool("Land", true);
						}
						transform.localPosition = Vector3.Lerp(lhs.position, rhs.position, t);
						this.transform.FindChild ("Aim").transform.position = Vector3.Lerp(lhs.aimpos, rhs.aimpos, t);
						return;
					}
				}
			}
			else
			{
				Debug.Log ("Je passe ici");
				StructCodec.ResultStruct latest  = proxyStates[0];
				transform.position = Vector3.Lerp(transform.position, latest.position, 0.5f);
				this.transform.FindChild ("Aim").transform.position = proxyStates[0].aimpos;
			}
		}
	}

	void uLink_OnSerializeNetworkViewOwner(uLink.BitStream stream, uLink.NetworkMessageInfo info) 
	{ 
		StructCodec.ResultStruct result;
		StructCodec.PlayerStateStruct playerstate;
		if (stream.isWriting) 
		{
			result =  this.GetComponent<ServerPLayer>().LastResult;
			playerstate = this.GetComponent<ServerPLayer>().PlayerState;
			stream.Write(result);
			stream.Write (playerstate);
            stream.Write(killmessage);
            killmessage = "";
		} 
		else 
		{ 
			string name = this.GetComponent<ClientPlayer>().PlayerState.nickname ;
			result = stream.Read<StructCodec.ResultStruct>();
			this.GetComponent<ClientPlayer>().PlayerState = stream.Read<StructCodec.PlayerStateStruct>();
			this.GetComponent<ClientPlayer>().PlayerState.nickname = name;
            killmessage = stream.Read<string>();
            if (killmessage != "")
                GameObject.Find("KillLog").GetComponent<KillLog>().kills.Enqueue(killmessage);
			Reconciliation(result);
		} 
	}

	private void Reconciliation(StructCodec.ResultStruct result)
	{
		Queue<StructCodec.InputStruct> inputarray =  this.GetComponent<ClientPlayer>().inputarray;
		bool found = false;
		int i = 0;
		foreach(StructCodec.InputStruct input in inputarray)
		{
			if (input.ID == result.ID)
			{
				found = true;
				break;
			}
			i++;
		}
		if (found == true) {
			while (i >= 1) {
				inputarray.Dequeue ();
				i--;
			}
			var _distance = Vector3.Distance (inputarray.Peek ().result, result.position);
			if (_distance >= 0.5f) {
				this.transform.position = result.position;
				CharacterMotor motor = this.GetComponent<ClientPlayer> ().motor;
				motor.verticalSpeed = result.VertcialSpeed;
				inputarray.Dequeue ();
				foreach (StructCodec.InputStruct a in inputarray) {
					if (a.jump == true)
						motor.jump (Time.time);
					motor.UpdateMovement (a.hor, (float)a.dt);
					a.result = this.transform.position;
				}
			}
		}
	}

	void uLink_OnSerializeNetworkView(uLink.BitStream stream, uLink.NetworkMessageInfo info) 
	{ 
		StructCodec.PlayerStateStruct playerstate;
		StructCodec.ResultStruct result;
		if (stream.isWriting) 
		{
			result =  this.GetComponent<ServerPLayer>().LastResult;
			playerstate = this.GetComponent<ServerPLayer>().PlayerState;
			stream.Write(result);
			stream.Write (playerstate);
		} 
		else 
		{
			string name = this.GetComponent<ProxyPlayer>().PlayerState.nickname ;
			result = stream.Read<StructCodec.ResultStruct>();
			this.GetComponent<ProxyPlayer>().PlayerState = stream.Read<StructCodec.PlayerStateStruct>();
			this.GetComponent<ProxyPlayer>().PlayerState.nickname = name;
			for (int i = proxyStates.Length - 1; i >= 1; i--)
			{
				proxyStates[i] = proxyStates[i - 1];
			}
			result.timestamp = info.rawServerTimestamp;
			proxyStates[0] = result;
			proxyStateCount = Mathf.Min(proxyStateCount + 1, proxyStates.Length);
		//	this.transform.position = result.position;
		} 
	} 
}
