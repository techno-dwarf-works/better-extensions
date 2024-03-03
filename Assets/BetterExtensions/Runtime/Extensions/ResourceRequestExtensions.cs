using System;
using Better.Extensions.Runtime.Helpers;
using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class ResourceRequestExtensions
    {
        public static ResourceRequestAwaiter GetAwaiter(this ResourceRequest self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            return new ResourceRequestAwaiter(self);
        }
    }
}