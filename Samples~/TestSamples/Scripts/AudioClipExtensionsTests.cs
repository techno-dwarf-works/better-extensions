using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Better.Extensions.Runtime;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Samples
{
    [Serializable]
    public class AudioClipExtensionsTests
    {
        [SerializeField] private AudioClip testClip;
        [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

        public List<AudioClip> AudioClips => audioClips;
        public event Action TestsDone;
        private int count = 0;

        public void Start()
        {
            audioClips.Clear();
            audioClips.Add(testClip);
            count = 0;
            TestAsync(testClip);
            Test(testClip);
        }

        private void Test(AudioClip testClass)
        {
            var debugString = new StringBuilder();
            debugString.AppendLine($"{nameof(AudioClipExtensionsTests)}.{nameof(Test)} started");

            var stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            var serialized = testClass.ToByteArray();
            stopwatch.Stop();

            debugString.AppendLine($"ToByteArray passed. This took {stopwatch.ElapsedMilliseconds.ToString()} ms");
            stopwatch.Reset();

            stopwatch.Start();
            var deserialize = serialized.FromByteArray($"{testClip.name}{nameof(Test)}", testClip.channels,
                testClip.frequency);
            stopwatch.Stop();

            debugString.AppendLine($"Deserialize passed. This took {stopwatch.ElapsedMilliseconds.ToString()} ms");
            stopwatch.Reset();
            debugString.AppendLine($"{nameof(AudioClip)}.{nameof(AudioClip.name)} = {deserialize.name}");

            audioClips.Add(deserialize);
            Debug.Log(debugString.ToString());
            CallTestsDone();
        }

        private async void TestAsync(AudioClip testClass)
        {
            var debugString = new StringBuilder();
            debugString.AppendLine($"{nameof(AudioClipExtensionsTests)}.{nameof(TestAsync)} started");

            var stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            var serialized = await testClass.ToByteArrayAsync();
            stopwatch.Stop();

            debugString.AppendLine($"ToByteArray passed. This took {stopwatch.ElapsedMilliseconds.ToString()} ms");
            stopwatch.Reset();

            stopwatch.Start();
            var deserialize = await serialized.FromByteArrayAsync($"{testClip.name}{nameof(TestAsync)}",
                testClip.channels, testClip.frequency);
            stopwatch.Stop();

            debugString.AppendLine($"Deserialize passed. This took {stopwatch.ElapsedMilliseconds.ToString()} ms");
            stopwatch.Reset();
            debugString.AppendLine($"{nameof(AudioClip)}.{nameof(AudioClip.name)} = {deserialize.name}");

            audioClips.Add(deserialize);
            Debug.Log(debugString.ToString());
            CallTestsDone();
        }

        private void CallTestsDone()
        {
            count++;
            if (count == 2)
            {
                TestsDone?.Invoke();
            }
        }
    }
}