using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class QuaternionExtensions
    {
        public static bool IsNormalized(this Quaternion self)
        {
            return QuaternionUtility.IsNormalized(self);
        }

        public static bool Approximately(this Quaternion self, Quaternion other)
        {
            return QuaternionUtility.Approximately(self, other);
        }

        public static Quaternion Scale(this Quaternion self, Vector3 scale)
        {
            return QuaternionUtility.Scale(self, scale);
        }

        public static Quaternion Scale(this Quaternion self, float scale)
        {
            return QuaternionUtility.Scale(self, scale);
        }
    }
}