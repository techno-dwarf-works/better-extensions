using System;
using System.Reflection;

namespace Better.Extensions.Runtime
{
    public static class MemberInfoExtensions
    {
        public static string PrettyMemberName(this MemberInfo self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return string.Empty;
            }
            
            return self.Name.PrettyCamelCase();
        }
    }
}