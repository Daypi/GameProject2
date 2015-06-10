using UnityEngine;
using System.Collections;

public class FPSCounter
{
	public float FPS = -1;
	private float nextUpdate = 0;
	private int frameCount = 0;
	public FPSCounter()
	{
		nextUpdate = Time.realtimeSinceStartup + 1.0f;
	}
	
	public void Update()
	{
		++frameCount;
		float dt = Time.realtimeSinceStartup - nextUpdate;
		if (dt >= 1.0f)
		{
			FPS = (float)frameCount / dt;
			
			frameCount = 0;
			nextUpdate += 1.0f; // this will cause real 1 sec intervals, ignoring the update rate,
		}
	}
}
