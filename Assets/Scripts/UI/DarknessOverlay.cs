using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DarknessOverlay : MonoBehaviour
{
    [SerializeField] Color from;
    [SerializeField] Color to;
    [SerializeField] float transitionTime;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    IEnumerator ColorTransition(Color from, Color to)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < transitionTime)
        {
            image.color = Color.Lerp(from, to, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void On()
    {
        StartCoroutine(ColorTransition(from, to));
    }

    public void Off()
    {
        StartCoroutine(ColorTransition(to, from));
    }
}
