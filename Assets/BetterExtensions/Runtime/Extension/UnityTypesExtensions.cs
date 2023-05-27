using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class UnityTypesExtensions
    {
        public static bool IsNullOrDestroyed(this Object obj)
        {
            if (ReferenceEquals(obj, null)) return true;

            return obj == null;
        }
    }
}