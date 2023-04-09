using System;
using System.Collections.Generic;
using System.Reflection;

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

            var type = listType.GetGenericTypeDefinition();
            if (listType.IsGenericType && type == typeof(List<>))
            {
                return listType.GetGenericArguments()[0];
            }

            return null;
        }

        public static bool IsArrayOrList(this FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                return false;
            return fieldInfo.FieldType.IsArrayOrList();
        }

        public static Type GetArrayOrListElementType(this FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                return null;
            return fieldInfo.FieldType.GetArrayOrListElementType();
        }
    }
}