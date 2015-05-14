using UnityEngine;
using System.Collections;

public class Predictor : MonoBehaviour {
	public Transform observedTransform ;
	public C_PlayerManager receiver ; //Guy who is receiving data
	public float pingMargin = 0.5f; //ping top-margin
	
	private float clientPing ;
	private NetState[] serverStateBuffer  = new NetState[20];
	// Use this for initialization
	void Start () {
	
	}

	public void OnSerializeNetworkView(BitStream stream , NetworkMessageInfo info) {
		Vector3 pos = observedTransform.position;
		Quaternion rot = observedTransform.rotation;
		
		if (stream.isWriting) {
			//Debug.Log("Server is writing");
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
		}
		else {
			//This code takes care of the local client!
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
			receiver.serverPos = pos;
			receiver.serverRot = rot;
			//Smoothly correct clients position
			receiver.lerpToTarget();
			
			//Take care of data for interpolating remote objects movements
			// Shift up the buffer
			for ( int i = serverStateBuffer.Length - 1; i >= 1; i-- ) {
				serverStateBuffer[i] = serverStateBuffer[i-1];
			}
			//Override the first element with the latest server info
			serverStateBuffer[0] = new NetState(info.timestamp, pos, rot);
		}
	}


	// Update is called once per frame
	void Update () {
		if ((Network.player == receiver.getOwner()) || Network.isServer) {
			return; //This is only for remote peers, get off
		}
		//client side has !!only the server connected!!
		clientPing = (Network.GetAveragePing(Network.connections[0]) / 100) + pingMargin;
		var interpolationTime = Network.time - clientPing;
		//ensure the buffer has at least one element:
		if (serverStateBuffer[0] == null) {
			serverStateBuffer[0] = new NetState(0, 
			                                    transform.position, 
			                                    transform.rotation);
		}
		//Try interpolation if possible. 
		//If the latest serverStateBuffer timestamp is smaller than the latency
		//we're not slow enough to really lag out and just extrapolate.
		if (serverStateBuffer[0].timestamp > interpolationTime) {
			for (int i  = 0; i < serverStateBuffer.Length; i++) {
				if (serverStateBuffer[i] == null) {
					continue;
				}
				// Find the state which matches the interp. time or use last state
				if (serverStateBuffer[i].timestamp <= interpolationTime|| 
				    i == serverStateBuffer.Length - 1) {
					
					// The state one frame newer than the best playback state
					NetState bestTarget  = serverStateBuffer[Mathf.Max(i-1, 0)];
					// The best playback state (closest current network time))
					NetState bestStart  = serverStateBuffer[i];
					
					double timediff  = bestTarget.timestamp - bestStart.timestamp;
					double lerpTime  = 0.0F;
					// Increase the interpolation amount by growing ping
					// Reverse that for more smooth but less accurate positioning
					if (timediff > 0.0001) {
						lerpTime = ((interpolationTime - bestStart.timestamp) / timediff);
					}
					
					transform.position = Vector3.Lerp(	bestStart.pos, 
					                                  bestTarget.pos, 
					                                  (float)lerpTime);
					
					transform.rotation = Quaternion.Slerp(	bestStart.rot, 
					                                      bestTarget.rot, 
					                                      (float)lerpTime);
					//Okay found our way through to lerp the positions, lets return here
					return;
				}
			}
		}
		//so it appears there is no lag through latency.
		else {
			NetState latest  = serverStateBuffer[0];	
			transform.position = Vector3.Lerp(transform.position, latest.pos, 0.5f);
			transform.rotation = Quaternion.Slerp(transform.rotation, latest.rot, 0.5f);
		}
	}
}
