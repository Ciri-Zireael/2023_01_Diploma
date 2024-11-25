using System.Collections;
using TMPro;
using UnityEngine;

public class TextTimer : MonoBehaviour
{
    TextMeshProUGUI text;
    Timer timer;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        timer = GetComponent<Timer>();
        
        timer.Run();
        StartCoroutine(UpdateText());
    }

    IEnumerator UpdateText()
    {
        while (true)
        {
            int secondsTotal = timer.GetSeconds();

            int seconds = secondsTotal % 60;
            int minutes = secondsTotal / 60;
            
            text.text = $"{minutes:D2}:{seconds:D2}";
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}
