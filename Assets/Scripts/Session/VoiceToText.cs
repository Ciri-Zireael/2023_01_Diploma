using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    MicrophoneRecord microphoneRecord;
    WhisperStream stream;

    string text;

    WhisperManager whisper;

    void Awake()
    {
        whisper = GetComponent<WhisperManager>();
        microphoneRecord = GetComponent<MicrophoneRecord>();
    }

    public void Reset()
    {
        StopListening();
        text = "";
        StartListening();
    }

    async void Start()
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
        string result = segment.Result;

        const string pattern = @"\[.*?\]|\(.*?\)";

        string filteredResult = Regex.Replace(result, pattern, "");

        text += filteredResult;

        if (logOutput)
        {
            Debug.Log(segment.Result);
        }
    }

    public Dictionary<string, int> GetSortedWordUsage()
    {
        string[] words = text.Split(new[] { ' ', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        var wordUsage = new Dictionary<string, int>();

        foreach (string word in words)
        {
            string lowercaseWord = word.ToLower();

            if (ignoreWords.Contains(lowercaseWord))
            {
                continue;
            }

            if (wordUsage.ContainsKey(lowercaseWord))
            {
                wordUsage[lowercaseWord]++;
            }
            else
            {
                wordUsage[lowercaseWord] = 1;
            }
        }

        Dictionary<string, int> sortedWordUsage = wordUsage.OrderByDescending(kv => kv.Value)
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        return sortedWordUsage;
    }

    public string GetRawText()
    {
        return text;
    }
}
