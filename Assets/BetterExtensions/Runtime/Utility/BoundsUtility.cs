using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class BoundsUtility
    {
        public static bool Approximately(Bounds current, Bounds other)
        {
            return current.center.Approximately(other.center) &&
                   current.size.Approximately(other.size);
        }
    }
}