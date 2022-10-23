using UnityEngine;
using UnityEngine.Networking;

namespace BetterExtensions.Runtime.Extension.AsyncExtension
{
    public static class ExtensionMethods
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
        {
            return new UnityWebRequestAwaiter(asyncOp);
        }

        public static AsyncOperationAwaiter GetAwaiter(this AsyncOperation asyncOp)
        {
            return new AsyncOperationAwaiter(asyncOp);
        }
        
        public static AssetBundleRequestAwaiter GetAwaiter(this AssetBundleRequest asyncOp)
        {
            return new AssetBundleRequestAwaiter(asyncOp);
        }
        
        public static ResourceRequestAwaiter GetAwaiter(this ResourceRequest asyncOp)
        {
            return new ResourceRequestAwaiter(asyncOp);
        }
    }
}