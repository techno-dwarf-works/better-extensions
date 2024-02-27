using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class BoundsExtensions
    {
        public static bool Approximately(this Bounds self, Bounds other)
        {
            return BoundsUtility.Approximately(self, other);
        }
    }
}