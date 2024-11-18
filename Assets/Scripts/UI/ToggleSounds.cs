using UnityEngine;
using UnityEngine.UI;

public class ToggleSounds : MonoBehaviour
{
    Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = PlayerPrefs.GetInt("Sounds", 1) == 1;
        
        toggle.onValueChanged.AddListener(delegate
        {
            PlayerPrefs.SetInt("Sounds", toggle.isOn ? 1 : 0);
            PlayerPrefs.Save(); 
        });
    }
}
