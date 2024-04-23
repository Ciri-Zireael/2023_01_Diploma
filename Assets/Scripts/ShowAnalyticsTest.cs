using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowAnalyticsTest : MonoBehaviour
{
	Text displayOn;

	void Awake()
	{
		displayOn = GetComponent<Text>();
	}

	public void Show(Dictionary<string, int> wordsUsed, float secondsPassed)
	{
		displayOn.text = "Time taken: " + secondsPassed + "s" + "\n \n \n";

		foreach (KeyValuePair<string, int> wordUsed in wordsUsed)
		{
			displayOn.text += wordUsed.Key + " was used " + wordUsed.Value + " times\n";
		}
	}
}
