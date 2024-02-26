namespace Better.Extensions.Runtime
{
    public static class IntExtensions
    {
        /// <summary>
        /// Check that point in two other points
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static bool InRange(this int value, int min, int max)
        {
            return IntUtility.InRange(value, min, max);
        }
    }
}