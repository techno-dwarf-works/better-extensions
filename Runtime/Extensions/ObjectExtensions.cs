namespace Better.Extensions.Runtime
{
    public static class ObjectExtensions
    {
        public static bool IsNullable<T>(this T self)
        {
            return TypeUtility.IsNullable<T>();
        }
    }
}