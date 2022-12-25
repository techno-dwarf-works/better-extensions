using System;

namespace Better.Extensions.Runtime
{
    public static class NullableExtensions
    {
        public static bool IsNullable<T>(this T obj)
        {
            return obj == null || IsNullable<T>();
        }

        public static bool IsNullable<T>()
        {
            var type = typeof(T);
            return type.IsNullable();
        }

        public static bool IsNullable(this Type type)
        {
            if (!type.IsValueType) return true;
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static bool Cast<T>(this object o, out T value)
        {
            value = default;
            if (IsNullable<T>())
            {
                if (o == null)
                {
                    return true;
                }
            }

            if (o is T casted)
            {
                value = casted;
                return true;
            }

            return false;
        }

        public static bool Cast<T1, T>(this T1 o, out T value)
        {
            value = default;
            if (IsNullable<T>())
            {
                if (o == null)
                {
                    return true;
                }
            }

            if (o is T casted)
            {
                value = casted;
                return true;
            }

            return false;
        }
    }
}