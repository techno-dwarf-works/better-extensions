using System;
using System.Threading.Tasks;
using Better.Extensions.Runtime.Helpers;
using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class AsyncOperationExtensions
    {
        public static bool IsRelativeCompleted(this AsyncOperation self)
        {
            return self.allowSceneActivation ? self.isDone : self.progress >= 0.9f;
        }

        public static AsyncOperationAwaiter GetAwaiter(this AsyncOperation self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            return new AsyncOperationAwaiter(self);
        }

        public static async Task AwaitCompletion(this AsyncOperation self, IProgress<float> progress = null)
        {
            var awaiter = new AsyncOperationCompletionAwaiter(self, progress);
            await awaiter.Task;
        }
    }
}