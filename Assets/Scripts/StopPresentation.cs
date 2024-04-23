using UnityEngine;
using UnityEngine.SceneManagement;

public class StopPresentation : MonoBehaviour, Interactive
{
	[SerializeField] string sceneToLoadName;
	[SerializeField] VoiceToText voiceToText;
	[SerializeField] Timer timer;

	void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == sceneToLoadName)
		{
			var showAnalyticsTest = FindObjectOfType<ShowAnalyticsTest>();

			if (showAnalyticsTest != null)
			{
				showAnalyticsTest.Show(voiceToText.GetSortedWordUsage(voiceToText.text), timer.GetSeconds());
			}
		}

		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void Interact()
	{
		voiceToText.StopListening();
		timer.Stop();
		
		SceneManager.LoadScene(sceneToLoadName);
	}
}
