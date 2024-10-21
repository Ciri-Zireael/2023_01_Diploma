using System;
using System.Threading.Tasks;
using UnityEngine;
using Whisper;
using Whisper.Utils;

namespace Session
{
    public class SpeechPaceTracker : MonoBehaviour
    {
        public MicrophoneRecord microphoneRecord;
        public WhisperStream stream;
        public WhisperManager whisper;
        
        private float speechStartTime;
        private float totalSpeechDuration;
        private int totalWordCount;


        async void Start()
        {
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
            microphoneRecord.OnVadChanged += HandleVadChanged;
            stream.OnSegmentFinished += HandleSegmentFinished;

            StartListening();
        }
        
        public void StartListening()
        {
            stream.StartStream();
            microphoneRecord.StartRecord();
        }

        private void HandleVadChanged(bool isSpeechDetected)
        {
            if (isSpeechDetected)
            {
                speechStartTime = Time.time; // Start timing
            }
            else
            {
                totalSpeechDuration += Time.time - speechStartTime; // Stop timing
            }
        }

        private void HandleSegmentFinished(WhisperResult result)
        {
            int wordCount = CountWords(result.Result); // Count words in the transcribed result
            totalWordCount += wordCount;

            Debug.Log($"Total Words: {totalWordCount}, Total Duration: {totalSpeechDuration} seconds, Pace: {CalculateSpeechPace()} words/sec");
        }

        private int CountWords(string text)
        {
            // Simple word count implementation
            if (string.IsNullOrWhiteSpace(text))
                return 0;
        
            return text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        private float CalculateSpeechPace()
        {
            if (totalSpeechDuration > 0)
            {
                return totalWordCount / totalSpeechDuration; // Words per second
            }
            return 0f;
        }
    }
}