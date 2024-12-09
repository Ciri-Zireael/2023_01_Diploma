using TMPro;
using UnityEngine;

public class AnalyticsPace : MonoBehaviour
{
	TextMeshProUGUI text;
	
	private void Start()
	{
		text = GetComponent<TextMeshProUGUI>();

		text.text = PlayerPrefs.GetFloat("Pace", 0).ToString("F1");
	}
}