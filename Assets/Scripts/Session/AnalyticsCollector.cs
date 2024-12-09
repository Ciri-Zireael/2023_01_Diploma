using UnityEngine;

public class AnalyticsCollector : MonoBehaviour
{
	[SerializeField] SpeechToText stt;
	[SerializeField] Timer timer;

	public void SaveAnalytics()
	{
		PlayerPrefs.SetString("Words", stt.GetSortedWordUsage(10));
		PlayerPrefs.SetFloat("Pace", stt.CalculateSpeechPace());
		PlayerPrefs.SetInt("Time", timer.GetSeconds());

		Debug.Log(PlayerPrefs.GetString("Words"));
		Debug.Log(PlayerPrefs.GetFloat("Pace"));
		Debug.Log(PlayerPrefs.GetInt("Time"));
		
		PlayerPrefs.Save();
	}
}
