using System;
using System.Diagnostics;
using System.Text;
using BetterExtension.Runtime;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace BetterExtension.Tests
{
    [Serializable]
    public class TestClass
    {
        public TestClass(string testString)
        {
            TestString = testString;
        }

        public string TestString { get; }
    }

    [Serializable]
    public class SerializeExtensionsTests
    {
        [TextArea(5, 10)]
        [SerializeField] private string testString = "Some test";
        public void Start()
        {
            var testClass = new TestClass(testString);
            TestAsync(testClass);
            Test(testClass);
        }

        private void Test(TestClass testClass)
        {
            var debugString = new StringBuilder();
            debugString.AppendLine($"{nameof(Test)} started");
            
            var stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            var serialized = testClass.Serialize();
            stopwatch.Stop();
            
            debugString.AppendLine($"Serialize passed. This took {stopwatch.ElapsedMilliseconds.ToString()} ms");
            stopwatch.Reset();
            
            stopwatch.Start();
            var compressed = serialized.Compress();
            stopwatch.Stop();
            
            debugString.AppendLine($"Compress passed. This took {stopwatch.ElapsedMilliseconds.ToString()} ms");
            stopwatch.Reset();
            
            stopwatch.Start();
            var decompressed = compressed.Decompress();
            stopwatch.Stop();
            
            debugString.AppendLine($"Decompress passed. This took {stopwatch.ElapsedMilliseconds.ToString()} ms");
            stopwatch.Reset();
            
            stopwatch.Start();
            var deserialize = decompressed.Deserialize<TestClass>();
            stopwatch.Stop();
            
            debugString.AppendLine($"Deserialize passed. This took {stopwatch.ElapsedMilliseconds.ToString()} ms");
            stopwatch.Reset();
            debugString.AppendLine($"{nameof(TestClass)}.{nameof(TestClass.TestString)} = {deserialize.TestString}");
            
            Debug.Log(debugString.ToString());
        }

        private async void TestAsync(TestClass testClass)
        {
            var debugString = new StringBuilder();
            debugString.AppendLine($"{nameof(TestAsync)} started");
            
            var stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            var serialized = await testClass.SerializeAsync();
            stopwatch.Stop();
            
            debugString.AppendLine($"SerializeAsync passed. This took {stopwatch.ElapsedMilliseconds.ToString()} ms");
            stopwatch.Reset();

            stopwatch.Start();
            var compressed = await serialized.CompressAsync();
            stopwatch.Stop();
            
            debugString.AppendLine($"CompressAsync passed. This took {stopwatch.ElapsedMilliseconds.ToString()} ms");
            stopwatch.Reset();
            
            stopwatch.Start();
            var decompressed = await compressed.DecompressAsync();
            stopwatch.Stop();
            
            debugString.AppendLine($"DecompressAsync passed. This took {stopwatch.ElapsedMilliseconds.ToString()} ms");
            stopwatch.Reset();
            
            stopwatch.Start();
            var deserialize = await decompressed.DeserializeAsync<TestClass>();
            stopwatch.Stop();
            
            debugString.AppendLine($"DeserializeAsync passed. This took {stopwatch.ElapsedMilliseconds.ToString()} ms");
            stopwatch.Reset();
            debugString.AppendLine($"{nameof(TestClass)}.{nameof(TestClass.TestString)} = {deserialize.TestString}");
            
            Debug.Log(debugString.ToString());
        }
    }
}