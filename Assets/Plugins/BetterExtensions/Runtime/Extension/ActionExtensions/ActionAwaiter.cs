using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Better.Extensions.Runtime.Extension.ActionExtensions
{
    public static class ActionAwaiter
    {
        public static Task Await(this UnityEvent action, CancellationToken cancellationToken = default)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            var buffer = new UnityEventWrapper(taskCompletionSource, ref action);
            if (!cancellationToken.Equals(default))
            {
                cancellationToken.Register(() => buffer.Cancel());
            }

            return buffer.Initialize();
        }
        
        public static Task Await<T>(this UnityEvent<T> action, CancellationToken cancellationToken = default)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            var buffer = new UnityEventWrapper<T>(taskCompletionSource, ref action);
            if (!cancellationToken.Equals(default))
            {
                cancellationToken.Register(() => buffer.Cancel());
            }

            return buffer.Initialize();
        }
    }
}