﻿using System;
using Object = UnityEngine.Object;

namespace Better.Extensions.Runtime
{
    public static class UnityObjectExtensions
    {
        public static bool IsNullOrDestroyed(this Object self)
        {
            if (ReferenceEquals(self, null))
            {
                return true;
            }

            return self == null;
        }

        /// <summary>
        /// Gets object name and converts into CamelCase
        /// </summary>
        /// <param name="self"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public static string GetPrettyObjectName(this Object self, params string[] remove)
        {
            if (self.IsNullOrDestroyed())
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return string.Empty;
            }

            if (remove != null)
            {
                foreach (var value in remove)
                {
                    self.name = self.name.Replace(value, string.Empty);
                }
            }

            return self.name.PrettyCamelCase();
        }
    }
}