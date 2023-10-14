﻿using System;
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

        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            if (type == typeof(string))
            {
                return string.Empty;
            }

            return null;
        }

        public static Type[] GetAllInheritedType(this Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => ValidateType(baseType, p)).ToArray();
        }
        
        public static Type[] GetAllInheritedTypeWithUnityObjects(this Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => ValidateTypeWithUnityObject(baseType, p)).ToArray();
        }

        private static bool ValidateTypeWithUnityObject(Type baseType, Type iterateValue)
        {
            return CheckType(baseType, iterateValue) &&
                   (iterateValue.IsClass || iterateValue.IsValueType) &&
                   !iterateValue.IsAbstract;
        }
        
        private static bool ValidateType(Type baseType, Type iterateValue)
        {
            return ValidateTypeWithUnityObject(baseType, iterateValue) && !iterateValue.IsSubclassOf(typeof(UnityEngine.Object));
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

        public static Type GetFieldOrElementType(this FieldInfo fieldInfo)
        {
            if (fieldInfo.IsArrayOrList())
                return fieldInfo.GetArrayOrListElementType();

            return fieldInfo.FieldType;
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