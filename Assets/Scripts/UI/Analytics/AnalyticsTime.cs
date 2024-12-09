using TMPro;
using UnityEngine;

public class AnalyticsTime : MonoBehaviour
{
	TextMeshProUGUI text;
	
	private void Start()
	{
		text = GetComponent<TextMeshProUGUI>();

		text.text = PlayerPrefs.GetInt("Time", 0).ToString();
	}
}