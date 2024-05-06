using UnityEngine;

public class Timer : MonoBehaviour
{
	int ticksPassed;
	bool isRunning;

	void FixedUpdate()
	{
		if (isRunning)
		{
			ticksPassed++;
		}
	}

	public float GetSeconds()
	{
		return ticksPassed * Time.fixedDeltaTime;
	}

	public void Start()
	{
		isRunning = true;
	}

	public void Stop()
	{
		isRunning = false;
	}

	public void Reset()
	{
		ticksPassed = 0;
	}
}
