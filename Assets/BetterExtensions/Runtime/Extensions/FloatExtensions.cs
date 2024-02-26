namespace Better.Extensions.Runtime
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Check that point in two other points
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static bool InRange(this float value, float min, float max)
        {
            return FloatUtility.InRange(value, min, max);
        }
    }
}