using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
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
    
    WhisperManager whisper;
    MicrophoneRecord microphoneRecord;
    WhisperStream stream;
    
    [HideInInspector] public string text;
    [ReadOnly] [SerializeField] string lastSegment;
    
    void Awake()
    {
        whisper = GetComponent<WhisperManager>();
        microphoneRecord = GetComponent<MicrophoneRecord>();
    }

    async void Start()
    {
        stream = await whisper.CreateStream(microphoneRecord);
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

    void Update()
    {
        DebugFrequencyTest();
    }

    // Just for testing purposes
    void DebugFrequencyTest()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            foreach (KeyValuePair<string, int> item in GetSortedWordUsage(text))
            {
                Debug.Log($"The word '{item.Key}' was used {item.Value} times");
            }
        }
    }

    public void Reset()
    {
        StopListening();
        text = "";
        lastSegment = "";
        StartListening();
    }

    void OnSegmentFinished(WhisperResult segment)
    {
        string result = segment.Result;
        
        string pattern = @"\[.*?\]|\(.*?\)";

        string filteredResult = Regex.Replace(result, pattern, "");

        text += filteredResult;
        lastSegment = segment.Result;

        if (logOutput)
        {
            Debug.Log(segment.Result);
        }
    }
    
    public Dictionary<string, int> GetSortedWordUsage(string text)
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
}