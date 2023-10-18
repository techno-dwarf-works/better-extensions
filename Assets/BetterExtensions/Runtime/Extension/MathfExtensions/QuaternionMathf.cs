using UnityEngine;

namespace Better.Extensions.Runtime.MathfExtensions
{
    public static class QuaternionMath
    {
        public static Quaternion Validate(this Quaternion rotation)
        {
            if (IsNormalized(rotation))
            {
                return rotation;
            }

            return Quaternion.identity;
        }

        public static bool IsNormalized(this Quaternion quaternion)
        {
            float magnitudeSquared = quaternion.x * quaternion.x + quaternion.y * quaternion.y + quaternion.z * quaternion.z + quaternion.w * quaternion.w;
            return Mathf.Approximately(magnitudeSquared, 1.0f);
        }
    }
}