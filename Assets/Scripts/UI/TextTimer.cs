using System.Collections;
using TMPro;
using UnityEngine;

public class TextTimer : MonoBehaviour
{
    TextMeshProUGUI text;
    Timer timer;
    int countDownFrom = 60;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        timer = GetComponent<Timer>();
        countDownFrom = PlayerPrefs.GetInt("Timer", 60);
        
        timer.Run();
        StartCoroutine(UpdateText());
    }

    IEnumerator UpdateText()
    {
        while (true)
        {
            int secondsTotal = countDownFrom - timer.GetSeconds();
            
            int seconds = secondsTotal % 60;
            int minutes = secondsTotal / 60;
            
            text.text = $"{minutes:D2}:{seconds:D2}";
            
            if (secondsTotal <= 0)
            {
                text.text = "00:00";
                break;
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}
