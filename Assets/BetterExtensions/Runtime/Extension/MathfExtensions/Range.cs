using System;
using UnityEngine;

namespace Better.Extensions.Runtime.MathfExtensions
{
    [Serializable]
    public struct Range<T> where T : new()
    {
        [SerializeField] private T min;
        [SerializeField] private T max;

        public T Min => min;
        public T Max => max;

        public Range(T minValue, T maxValue)
        {
            min = minValue;
            max = maxValue;
        }

        public Range<T> UpdateMax(T maxValue)
        {
            max = maxValue;
            return this;
        }

        public Range<T> UpdateMin(T minValue)
        {
            min = minValue;
            return this;
        }
    }
}