using TMPro;
using UnityEngine;

public class AnalyticsWords : MonoBehaviour
{
	TextMeshProUGUI text;
	
	private void Start()
	{
		text = GetComponent<TextMeshProUGUI>();

		text.text = PlayerPrefs.GetString("Words", "");
	}
}