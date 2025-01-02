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

        Debug.Log(PlayerPrefs.GetString("Words"));
        Debug.Log(PlayerPrefs.GetFloat("Pace"));
        Debug.Log(PlayerPrefs.GetInt("Time"));
        Debug.Log(PlayerPrefs.GetInt("ShowAnalytics"));
		
        PlayerPrefs.Save();
    }
}