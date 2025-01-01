using UnityEngine;

public class SoundManager : MonoBehaviour
{
	AudioSource source;
	bool isOn;

	void Awake()
	{
		source = GetComponent<AudioSource>();
		isOn = PlayerPrefs.GetInt("Sounds", 1) == 1;

		if (isOn)
		{
			source.Play();
		}
	}
}
