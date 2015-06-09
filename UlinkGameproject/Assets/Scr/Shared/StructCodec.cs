using UnityEngine;
using System.Collections;

public class StructCodec : uLink.MonoBehaviour {

public class InputStruct
	{
		public float ID;
		public float hor;
		public bool jump = false;
		public bool shoot = false;
		public bool reload = false;
		public bool heal = false;
		public float changeweapon;
		public float dt;
		public Vector3 result;
		public Vector3 aimpos;
		public uLink.NetworkMessageInfo info;
		public static void WriteMyClass(uLink.BitStream stream, object value,
		                                params object[] codecOptions)
		{
			InputStruct myObj = (InputStruct) value;
			stream.Write<float>(myObj.hor);
			stream.Write<bool>(myObj.jump);
			stream.Write<bool>(myObj.shoot);
			stream.Write<bool>(myObj.reload);
			stream.Write<bool>(myObj.heal);
			stream.Write<float>(myObj.changeweapon);
			stream.Write<float>(myObj.ID);
			stream.Write<Vector3>(myObj.aimpos);
		}

		public static object ReadMyClass(uLink.BitStream stream,
		                                 params object[] codecOptions)
		{
			InputStruct myObj = new InputStruct();
			myObj.hor = stream.Read<float>();
			myObj.jump = stream.Read<bool>();
			myObj.shoot = stream.Read<bool>();
			myObj.reload = stream.Read<bool>();
			myObj.heal = stream.Read<bool>();
			myObj.changeweapon = stream.Read<float>();
			myObj.ID = stream.Read<float>();
			myObj.aimpos = stream.Read<Vector3>();
			return myObj;
		}
	}

	public class ResultStruct
	{
		public float ID;
		public Vector3 position;
		public double timestamp;
		public Vector3 aimpos;
		public float VertcialSpeed;
		public static void WriteMyClass(uLink.BitStream stream, object value,
		                                params object[] codecOptions)
		{
			ResultStruct myObj = (ResultStruct) value;
			stream.Write<float>(myObj.ID);
			stream.Write<Vector3>(myObj.position);
			stream.Write<Vector3> (myObj.aimpos);
			stream.Write<float> (myObj.VertcialSpeed);
		}
		
		public static object ReadMyClass(uLink.BitStream stream,
		                                 params object[] codecOptions)
		{
			ResultStruct myObj = new ResultStruct();
			myObj.ID = stream.Read<float>();
			myObj.position = stream.Read<Vector3>();
			myObj.aimpos = stream.Read<Vector3>();
			myObj.VertcialSpeed = stream.Read<float>();
			return myObj;
		}
	}

	public class PlayerStateStruct
	{
		public int life = 0;
		public bool isdead = false;
		public string nickname = "";
		public bool facing = false;
		public int weapon = 0;
		public int NbDead = 0;
		public int NbKill = 0;
		public static void WriteMyClass(uLink.BitStream stream, object value,
		                                params object[] codecOptions)
		{
			PlayerStateStruct myObj = (PlayerStateStruct) value;
			stream.Write<int>(myObj.life);
			stream.Write<bool>(myObj.isdead);
			//stream.Write<bool> (myObj.facing);
			stream.Write<int> (myObj.weapon);
			stream.Write<int> (myObj.NbDead);
			stream.Write<int> (myObj.NbKill);
		}
		
		public static object ReadMyClass(uLink.BitStream stream,
		                                 params object[] codecOptions)
		{
			PlayerStateStruct myObj = new PlayerStateStruct();
			myObj.life = stream.Read<int>();
			myObj.isdead = stream.Read<bool>();
			//myObj.facing = stream.Read<bool>();
			myObj.weapon = stream.Read<int>();
			myObj.NbDead = stream.Read<int>();
			myObj.NbKill = stream.Read<int>();
			return myObj;
		}
	}

	void Awake()
	{
		// This method call must be made on both the client and the server.
		uLink.BitStreamCodec.Add<InputStruct>(InputStruct.ReadMyClass, InputStruct.WriteMyClass);
		uLink.BitStreamCodec.Add<ResultStruct>(ResultStruct.ReadMyClass, ResultStruct.WriteMyClass);
		uLink.BitStreamCodec.Add<PlayerStateStruct>(PlayerStateStruct.ReadMyClass, PlayerStateStruct.WriteMyClass);
	}
	
}
