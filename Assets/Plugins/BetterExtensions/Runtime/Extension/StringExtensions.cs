using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BetterExtensions.Runtime.Extension
{
    public static class StringExtensions
    {
        /// <summary>
        /// Makes first char upper
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null:
                    throw new ArgumentNullException(nameof(input));
                case "":
                    throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default:
                    return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
        
        public static string PrettyMemberName(this MemberInfo input)
        {
            return input.Name.PrettyCamelCase();
        }

        /// <summary>
        /// Gets object name and converts into CamelCase
        /// </summary>
        /// <param name="input"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public static string PrettyObjectName(this UnityEngine.Object input, params string[] remove)
        {
            if (remove == null) return input.name.PrettyCamelCase();
            foreach (var s in remove) input.name = input.name.Replace(s, string.Empty);
            return input.name.PrettyCamelCase();
        }

        public static string PrettyCamelCase(this string input)
        {
            return Regex.Replace(input.Replace("_", ""), "((?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z]))", " $1").Trim();
        }

        public static string ToTitleCase(this string input)
        {
            return Regex.Replace(input, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
        }
    }
}