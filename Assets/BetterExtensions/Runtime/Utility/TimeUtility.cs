using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class TimeUtility
    {
        public static int SecondsToMilliseconds(float seconds)
        {
            return Mathf.RoundToInt(seconds * 1000);
        }
    }
}