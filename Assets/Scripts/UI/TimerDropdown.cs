using TMPro;
using UnityEngine;

public class TimerDropdown : MonoBehaviour
{
    TMP_Dropdown dropdown;
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.value = PlayerPrefs.GetInt("TimerPreset");
        dropdown.RefreshShownValue();
    }

    public void OnDropdownValueChanged(int value)
    {
        switch (value)
        {
            case 0:
                PlayerPrefs.SetInt("Timer", 60);
                break;
            case 1:
                PlayerPrefs.SetInt("Timer", 300);
                break;
            case 2:
                PlayerPrefs.SetInt("Timer", 900);
                break;
        }
        PlayerPrefs.SetInt("TimerPreset", value);
        PlayerPrefs.Save();
    }
}
