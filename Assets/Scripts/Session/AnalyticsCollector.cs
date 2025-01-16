using UnityEngine;

public class AnalyticsCollector : MonoBehaviour
{
    [SerializeField] SpeechToText stt;
    [SerializeField] Timer timer;

    public void SaveAnalytics()
    {
        PlayerPrefs.SetString("Words", stt.GetSortedWordUsage(5));
        PlayerPrefs.SetFloat("Pace", stt.CalculateSpeechPace());
        PlayerPrefs.SetInt("Time", timer.GetSeconds());
        
        PlayerPrefs.SetInt("ShowAnalytics", 1);
		
        PlayerPrefs.Save();
    }
}