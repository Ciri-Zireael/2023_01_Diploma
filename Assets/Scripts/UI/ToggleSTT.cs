using UnityEngine;
using Toggle = UnityEngine.UI.Toggle;


public class ToggleSTT : MonoBehaviour
{
	Toggle toggle;

	void Start()
	{
		toggle = GetComponent<Toggle>();
		toggle.isOn = PlayerPrefs.GetInt("STT", 1) == 1;

		toggle.onValueChanged.AddListener(delegate
		{
			PlayerPrefs.SetInt("STT", toggle.isOn ? 1 : 0);
			PlayerPrefs.Save(); 
		});
	}
}
