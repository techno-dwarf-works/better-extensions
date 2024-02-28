using System;
using Better.Extensions.Runtime.Helpers;
using UnityEngine.Networking;

namespace Better.Extensions.Runtime
{
    public static class UnityWebRequestAsyncOperationExtensions
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            return new UnityWebRequestAwaiter(self);
        }
    }
}