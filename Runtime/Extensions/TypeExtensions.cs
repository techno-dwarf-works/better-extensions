using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityObject = UnityEngine.Object;

namespace Better.Extensions.Runtime
{
    public static class TypeExtensions
    {
        public static bool IsArrayOrList(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (self.IsArray)
            {
                return true;
            }

            var listType = typeof(List<>);
            return self.IsGeneric(listType);
        }

        public static bool IsEnumerable(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (self.IsArray)
            {
                return true;
            }

            var enumerableType = typeof(IEnumerable<>);
            return self.IsGeneric(enumerableType);
        }

        public static bool IsGeneric(this Type self, Type type)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (self.IsGenericType && self.GetGenericTypeDefinition() == type)
            {
                return true;
            }

            return false;
        }

        public static bool IsGeneric<T>(this Type self)
        {
            var type = typeof(T);
            return self.IsGeneric(type);
        }

        public static bool IsEnum(this Type self, bool checkFlagAttribute = false)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (self.IsEnum)
            {
                return !checkFlagAttribute || self.GetCustomAttribute<FlagsAttribute>() != null;
            }

            return false;
        }

        public static object GetDefault(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            if (self.IsValueType)
            {
                return Activator.CreateInstance(self);
            }

            if (self == typeof(string))
            {
                return string.Empty;
            }

            return null;
        }

        public static IEnumerable<Type> GetAllInheritedTypes(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(self));
        }

        public static IEnumerable<Type> GetAllInheritedTypes(this Type self, params Type[] excludes)
        {
            return self.GetAllInheritedTypes().Except(excludes);
        }

        public static IEnumerable<Type> GetAllInheritedTypesWithoutUnityObject(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            var unityObjectType = typeof(UnityObject);
            if (self.IsSubclassOf(unityObjectType))
            {
                return Enumerable.Empty<Type>();
            }

            return self.GetAllInheritedTypes(unityObjectType);
        }

        public static bool IsSubclassOf<T>(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            var type = typeof(T);
            return self.IsSubclassOf(type);
        }

        public static bool IsSubclassOfAny(this Type self, IEnumerable<Type> anyTypes)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (anyTypes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(anyTypes));
                return false;
            }

            foreach (var anyType in anyTypes)
            {
                if (anyType != null && self.IsSubclassOf(anyType))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsSubclassOfRawGeneric(this Type self, Type genericType)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (genericType == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(genericType));
                return false;
            }

            while (self != null && self != typeof(object))
            {
                var definition = self.IsGenericType ? self.GetGenericTypeDefinition() : self;
                if (genericType == definition && self != genericType)
                {
                    return true;
                }

                self = self.BaseType;
            }

            return false;
        }
        
        public static bool IsSubclassOfAnyRawGeneric(this Type self, IEnumerable<Type> genericTypes)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (genericTypes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(genericTypes));
                return false;
            }

            foreach (var anyType in genericTypes)
            {
                if (anyType != null && self.IsSubclassOfRawGeneric(anyType))
                {
                    return true;
                }
            }

            return false;
        }

        public static Type GetElementType(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            if (self.IsArray)
            {
                return self.GetElementType();
            }

            if (self.IsEnumerable())
            {
                return self.GetGenericArguments()[0];
            }

            return null;
        }

        public static bool IsNullable(this Type self)
        {
            if (self == null || !self.IsValueType)
            {
                return true;
            }

            return Nullable.GetUnderlyingType(self) != null;
        }
    }
}