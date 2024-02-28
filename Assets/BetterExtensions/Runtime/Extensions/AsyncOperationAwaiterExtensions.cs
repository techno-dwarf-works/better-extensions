using System;
using Better.Extensions.Runtime.Helpers;
using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class AsyncOperationAwaiterExtensions
    {
        public static AsyncOperationAwaiter GetAwaiter(this AsyncOperation self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            return new AsyncOperationAwaiter(self);
        }
    }
}