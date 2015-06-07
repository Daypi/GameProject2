using UnityEngine;
using System.Collections;

public class StructCodec : uLink.MonoBehaviour {

public class InputStruct
	{
		public float ID;
		public float hor;
		public bool jump;
		public bool shoot;
		public float changeweapon;
		public float dt;
		public Vector3 result;
		public Vector3 aimpos;
		public static void WriteMyClass(uLink.BitStream stream, object value,
		                                params object[] codecOptions)
		{
			InputStruct myObj = (InputStruct) value;
			stream.Write<float>(myObj.hor);
			stream.Write<bool>(myObj.jump);
			stream.Write<bool>(myObj.shoot);
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
		public static void WriteMyClass(uLink.BitStream stream, object value,
		                                params object[] codecOptions)
		{
			ResultStruct myObj = (ResultStruct) value;
			stream.Write<float>(myObj.ID);
			stream.Write<Vector3>(myObj.position);
			stream.Write<Vector3> (myObj.aimpos);
		}
		
		public static object ReadMyClass(uLink.BitStream stream,
		                                 params object[] codecOptions)
		{
			ResultStruct myObj = new ResultStruct();
			myObj.ID = stream.Read<float>();
			myObj.position = stream.Read<Vector3>();
			myObj.aimpos = stream.Read<Vector3>();
			return myObj;
		}
	}

	public class PlayerStateStruct
	{
		public int life;
		public bool isdead = false;
		public string nickname = "";
		public bool facing;
	}

	void Awake()
	{
		// This method call must be made on both the client and the server.
		uLink.BitStreamCodec.Add<InputStruct>(InputStruct.ReadMyClass, InputStruct.WriteMyClass);
		uLink.BitStreamCodec.Add<ResultStruct>(ResultStruct.ReadMyClass, ResultStruct.WriteMyClass);
	}
	
}
