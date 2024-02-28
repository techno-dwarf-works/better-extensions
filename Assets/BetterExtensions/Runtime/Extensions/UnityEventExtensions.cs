using System;
using System.Threading;
using System.Threading.Tasks;
using Better.Extensions.Runtime.Helpers;
using UnityEngine.Events;

namespace Better.Extensions.Runtime
{
    public static class UnityEventExtensions
    {
        public static Task Await(this UnityEvent self, CancellationToken cancellationToken = default)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return Task.CompletedTask;
            }

            var completionAwaiter = new UnityEventCompletionAwaiter(self, cancellationToken);
            return completionAwaiter.Task;
        }

        public static Task<T> Await<T>(this UnityEvent<T> self, CancellationToken cancellationToken = default)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return Task.FromResult(default(T));
            }

            var completionAwaiter = new UnityEventCompletionAwaiter<T>(self, cancellationToken);
            return completionAwaiter.Task;
        }
    }
}