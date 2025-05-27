using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCheckData : MonoSingleton<EventCheckData>
{
    private int jumpCount = 0;
	private float timeCount = 0;
    private int pointCount = 0;

	private void Update()
	{
		AddTimeCount();
	}

	public int JumpCount
	{
		get => jumpCount;
	}
	public float TimeCount
	{
		get => timeCount;
	}
	public int PointCount
	{
		get => pointCount;
	}

	public void ResetEventData()
	{
		jumpCount = 0;
		timeCount = 0;
		pointCount = 0;
	}

	public void AddJumpCount()
	{
		jumpCount++;
	}
	public void AddTimeCount()
	{
		timeCount += Time.deltaTime;
	}
	public void AddPointCount()
	{
		pointCount++;
	}
}
