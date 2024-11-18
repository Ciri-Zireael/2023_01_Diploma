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

	public int GetSeconds()
	{
		return Mathf.FloorToInt(ticksPassed * Time.fixedDeltaTime);
	}
	
	public void Run()
	{
		isRunning = true;
	}

	public void Pause()
	{
		isRunning = false;
	}

	public void Reset()
	{
		ticksPassed = 0;
	}
}
