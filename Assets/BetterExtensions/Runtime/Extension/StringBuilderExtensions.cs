using System.Text;

namespace Better.Extensions.Runtime
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendLine(this StringBuilder builder, int value)
        {
            return builder.AppendLine(value.ToString());
        }
    }
}