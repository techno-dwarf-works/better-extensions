using System;
using System.Text;

namespace Better.Extensions.Runtime
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendLine(this StringBuilder self, object value)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            if (value == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(value));
                return self;
            }

            return self.AppendLine(value.ToString());
        }
    }
}