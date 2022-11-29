using UnityEngine;

namespace Better.Extensions.Runtime.Extension.MathfExtensions
{
    public static class RangeExtensions
    {
        public static int Clamp(this Range<int> range, int value)
        {
            return Mathf.Clamp(value, range.Min, range.Max);
        }
        
        public static float Clamp(this Range<float> range, float value)
        {
            return Mathf.Clamp(value, range.Min, range.Max);
        }
    }
}