using System;
using System.Threading;
using UnityEngine;
using ThreadingTask = System.Threading.Tasks.Task;

namespace Better.Extensions.Runtime.Helpers
{
    internal class AsyncOperationCompletionAwaiter : BaseCompletionAwaiter<bool>
    {
        private readonly IProgress<float> _progress;

        public AsyncOperationCompletionAwaiter(AsyncOperation sourceOperation, IProgress<float> progress = null)
            : base(CancellationToken.None)
        {
            _progress = progress;
            ProcessAsync(sourceOperation);
        }

        private async void ProcessAsync(AsyncOperation asyncOperation)
        {
            while (!asyncOperation.IsRelativeCompleted())
            {
                _progress?.Report(asyncOperation.progress);
                await ThreadingTask.Yield();
            }

            SetResult(true);
        }

        protected override void OnCompleted(bool result)
        {
            _progress?.Report(1f);
        }
    }
}