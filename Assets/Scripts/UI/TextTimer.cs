using System.Collections;
using TMPro;
using UnityEngine;

public class TextTimer : MonoBehaviour
{
    TextMeshProUGUI text;
    Timer timer;
    HintSystem hintSystem;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        timer = GetComponent<Timer>();
        hintSystem = GetComponent<HintSystem>();
        
        timer.Run();
        StartCoroutine(UpdateText());
        hintSystem.ShowHint("Haaaai :3");
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
