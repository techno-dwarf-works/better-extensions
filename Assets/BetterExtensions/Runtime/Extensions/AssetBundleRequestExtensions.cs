using System;
using Better.Extensions.Runtime.Helpers;
using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class AssetBundleRequestExtensions
    {
        public static AssetBundleRequestAwaiter GetAwaiter(this AssetBundleRequest self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            return new AssetBundleRequestAwaiter(self);
        }
    }
}