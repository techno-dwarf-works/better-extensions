using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public static Type[] GetAllInheritedType(this Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => ArgIsValueType(baseType, p)).ToArray();
        }

        private static bool ArgIsValueType(Type baseType, Type iterateValue)
        {
            return CheckType(baseType, iterateValue) &&
                   (iterateValue.IsClass || iterateValue.IsValueType) &&
                   !iterateValue.IsAbstract && !iterateValue.IsSubclassOf(typeof(UnityEngine.Object));
        }

        private static bool CheckType(Type baseType, Type p)
        {
            return baseType.IsAssignableFrom(p);
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