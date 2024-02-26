using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class BoundsUtility
    {
        public static bool Approximately(Bounds current, Bounds other)
        {
            return VectorUtility.Approximately(current.center, other.center) &&
                   VectorUtility.Approximately(current.size, other.size);
        }
    }
}