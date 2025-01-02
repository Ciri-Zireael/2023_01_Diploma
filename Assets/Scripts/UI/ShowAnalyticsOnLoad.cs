using System;
using UnityEngine;

public class ShowAnalyticsOnLoad : MonoBehaviour
{
    [SerializeField] GameObject toHide;
    [SerializeField] GameObject toShow;

    private void Start()
    {
        if (PlayerPrefs.GetInt("ShowAnalytics") == 1)
        {
            toHide.SetActive(false);
            toShow.SetActive(true);
        }
        PlayerPrefs.SetInt("ShowAnalytics", 0);
        PlayerPrefs.Save();
    }
}