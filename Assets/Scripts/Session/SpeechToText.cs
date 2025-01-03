using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using Whisper;
using Whisper.Utils;

[RequireComponent(typeof(WhisperManager))]
[RequireComponent(typeof(MicrophoneRecord))]
public class SpeechToText : MonoBehaviour
{
	[SerializeField] string[] ignoreWords =
	{
		"a", "an", "the", "at", "by",
		"of", "on", "to", "it",
		"is", "am", "are", "be",
		"of", "to", "its", "it's"
	};

	[SerializeField] bool logOutput;
	public bool isOn { get; private set; }
	MicrophoneRecord microphoneRecord;
	WhisperStream stream;
	HintSystem hintSystem;
	[SerializeField] float minPaceForHint = 0.15f;
	[SerializeField] string hintTextForMinPace;
	[SerializeField] string hintTextForMaxPace;
	[SerializeField] float maxPaceForHint = 1.5f;
	[SerializeField] int hintGapInSeconds = 15;
	float secsSinceLastHint;
	int totalWordCount;
	float speechStartTime;
	float totalSpeechDuration;

	string text;

	WhisperManager whisper;

	void Awake()
	{
		isOn = PlayerPrefs.GetInt("STT", 1) == 1;
		
		if (!isOn) return;
		Init();
	}

	void FixedUpdate()
	{
		secsSinceLastHint += Time.fixedDeltaTime;
	}

	async void Reset()
	{
		StopListening();

		Init();
		await InitStream();
	}

	async void Start()
	{
		if (!isOn) return;
		await InitStream();
	}

	async Task InitStream()
	{
		stream = await whisper.CreateStream(microphoneRecord);
		if (stream == null)
		{
			Debug.LogWarning("No microphone is connected or no model was found. " +
			                 "Please go to https://huggingface.co/ggerganov/whisper.cpp/tree/main and download a model. " +
			                 "Then put it into Assets/StreamingAssets/Whisper/");
			return;
		}

		stream.OnSegmentFinished += OnSegmentFinished;
		microphoneRecord.OnVadChanged += HandleVadChanged;

		StartListening();
	}

	void Init()
	{
		whisper = GetComponent<WhisperManager>();
		microphoneRecord = GetComponent<MicrophoneRecord>();
		hintSystem = GetComponent<HintSystem>();
		text = "";
	}
	
	public void StartListening()
	{
		stream.StartStream();
		microphoneRecord.StartRecord();
	}

	public void StopListening()
	{
		stream.OnSegmentFinished -= OnSegmentFinished;
		microphoneRecord.OnVadChanged -= HandleVadChanged;
		
		stream.StopStream();
		microphoneRecord.StopRecord();
	}

	void HandleVadChanged(bool isSpeechDetected)
	{
		if (isSpeechDetected)
		{
			speechStartTime = Time.time;
		}
		else
		{
			totalSpeechDuration += Time.time - speechStartTime;
		}
	}
	
	void OnSegmentFinished(WhisperResult segment)
	{
		var result = segment.Result;

		const string pattern = @"\[.*?\]|\(.*?\)|\*.*?\*";

		var filteredResult = Regex.Replace(result, pattern, "");

		text += filteredResult;

		if (logOutput) Debug.Log(segment.Result);
		
		int wordCount = CountWords(filteredResult); // Count words in the transcribed result
		totalWordCount += wordCount;

		float pace = CalculateSpeechPace();
		
		if (logOutput) Debug.Log($"Total Words: {totalWordCount}, Total Duration: {totalSpeechDuration} seconds, Pace: {pace} words/sec");

		Debug.Log("Secs since last hint: " + secsSinceLastHint);
		if (secsSinceLastHint < hintGapInSeconds) return;
		if (pace < minPaceForHint)
		{
			hintSystem.ShowHint(hintTextForMinPace);
			secsSinceLastHint = 0;
		}
		if (pace > maxPaceForHint)
		{
			hintSystem.ShowHint(hintTextForMaxPace);
			secsSinceLastHint = 0;
		}
	}
	
	int CountWords(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return 0;
		}
        
		return text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
	}
	
	float CalculateSpeechPace()
	{
		if (totalSpeechDuration > 0)
		{
			return totalWordCount / totalSpeechDuration; // Words per second
		}
		return 0f;
	}
	
	public Dictionary<string, int> GetSortedWordUsage()
	{
		var words = text.Split(new[] { ' ', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
		var wordUsage = new Dictionary<string, int>();

		foreach (var word in words)
		{
			var lowercaseWord = word.ToLower();

			if (ignoreWords.Contains(lowercaseWord)) continue;

			if (wordUsage.ContainsKey(lowercaseWord))
				wordUsage[lowercaseWord]++;
			else
				wordUsage[lowercaseWord] = 1;
		}

		var sortedWordUsage = wordUsage.OrderByDescending(kv => kv.Value)
			.ToDictionary(kv => kv.Key, kv => kv.Value);

		return sortedWordUsage;
	}

	void OnDestroy()
	{
		StopListening();
	}

	public string GetRawText()
	{
		return text;
	}
}