using UnityEngine;
using Toggle = UnityEngine.UI.Toggle;


public class ToggleSTT : MonoBehaviour
{
	Toggle toggle;

	private void Start()
	{
		toggle = GetComponent<Toggle>();
		toggle.isOn = VoiceToText.IsOn;
	}
}
