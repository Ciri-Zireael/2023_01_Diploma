using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using Whisper;
using Whisper.Utils;

[RequireComponent(typeof(WhisperManager))]
[RequireComponent(typeof(MicrophoneRecord))]
public class VoiceToText : MonoBehaviour
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

	string text;

	WhisperManager whisper;

	void Awake()
	{
		isOn = PlayerPrefs.GetInt("STT", 1) == 1;
		
		if (!isOn) return;
		Init();
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

		StartListening();
	}

	void Init()
	{
		whisper = GetComponent<WhisperManager>();
		microphoneRecord = GetComponent<MicrophoneRecord>();
		text = "";
	}
	

	public void StartListening()
	{
		stream.StartStream();
		microphoneRecord.StartRecord();
	}

	public void StopListening()
	{
		stream.StopStream();
		microphoneRecord.StopRecord();
	}

	void OnSegmentFinished(WhisperResult segment)
	{
		var result = segment.Result;

		const string pattern = @"\[.*?\]|\(.*?\)";

		var filteredResult = Regex.Replace(result, pattern, "");

		text += filteredResult;

		if (logOutput) Debug.Log(segment.Result);
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