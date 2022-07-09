using System;
using UnityEngine;

namespace BetterExtension.Tests
{
    public class Tester : MonoBehaviour
    {
        [SerializeField] private SerializeExtensionsTests serializeExtensionsTests = new SerializeExtensionsTests();
        [SerializeField] private bool start;

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                if (start)
                    serializeExtensionsTests.Start();
            }
        }
    }
}