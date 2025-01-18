using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarsSlider : MonoBehaviour
{
    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("AvatarsSlider");
    }

    public void OnSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat("AvatarsSlider", value);
        PlayerPrefs.Save();
    }
}
