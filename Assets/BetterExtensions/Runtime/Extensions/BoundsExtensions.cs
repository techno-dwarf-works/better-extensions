using UnityEngine;

namespace Better.Extensions.Runtime.Runtime.Extensions
{
    public static class BoundsExtensions
    {
        public static bool Approximately(this Bounds current, Bounds other)
        {
            return BoundsUtility.Approximately(current, other);
        }
    }
}