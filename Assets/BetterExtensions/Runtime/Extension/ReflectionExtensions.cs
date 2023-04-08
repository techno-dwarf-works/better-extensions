using System;
using System.Collections.Generic;

namespace Better.Extensions.Runtime
{
    public static class ReflectionExtensions
    {
        public static bool IsArrayOrList(this Type listType)
        {
            if (listType.IsArray)
            {
                return true;
            }

            if (listType.IsGenericType && listType.GetGenericTypeDefinition() == typeof(List<>))
            {
                return true;
            }
            return false;
        }

        public static Type GetArrayOrListElementType(this Type listType)
        {
            if (listType.IsArray)
            {
                return listType.GetElementType();
            }

            if (listType.IsGenericType && listType.GetGenericTypeDefinition() == typeof(List<>))
            {
                return listType.GetGenericArguments()[0];
            }
            return null;
        }
    }
}