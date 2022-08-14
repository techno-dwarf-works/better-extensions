using System.Collections;
using UnityEngine;

namespace Samples
{
    public class Tester : MonoBehaviour
    {
        [SerializeField] private SerializeExtensionsTests serializeExtensionsTests = new SerializeExtensionsTests();
        [SerializeField] private AudioClipExtensionsTests audioClipExtensionsTests = new AudioClipExtensionsTests();
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private bool start;
        private Coroutine coroutine;

        private void Awake()
        {
            audioClipExtensionsTests.TestsDone += TestsDone;
        }

        private void TestsDone()
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(AudioCoroutine());
        }

        private IEnumerator AudioCoroutine()
        {
            audioSource.Stop();
            foreach (var audioClip in audioClipExtensionsTests.AudioClips)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
                yield return new WaitWhile(() => audioSource.isPlaying);
            }
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                if (start)
                {
                    serializeExtensionsTests.Start();
                    audioClipExtensionsTests.Start();
                }
            }
        }
    }
}