using System;
using System.Reflection;

namespace Better.Extensions.Runtime
{
    public static class ExceptionExtensions
    {
        private const string ExceptionMessageFieldName = "_message";
        private static readonly FieldInfo _messageField;

        static ExceptionExtensions()
        {
            _messageField = GetExceptionMessageField();
        }

        private static FieldInfo GetExceptionMessageField()
        {
            var type = typeof(Exception);
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            return type.GetField(ExceptionMessageFieldName, flags);
        }

        public static void ReplaceExceptionMessageField(this Exception exception, string message)
        {
            _messageField.SetValue(exception, message);
        }

        public static void ReplaceExceptionMessageField(this Exception exception, object message)
        {
            exception.ReplaceExceptionMessageField(message.ToString());
        }
    }
}