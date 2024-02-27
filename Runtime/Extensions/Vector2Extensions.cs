using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class Vector2Extensions
    {
        public static bool Approximately(this Vector2 self, Vector2 other)
        {
            return Vector2Utility.Approximately(self, other);
        }
    }
}