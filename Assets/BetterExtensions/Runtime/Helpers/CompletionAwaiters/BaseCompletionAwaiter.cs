using System.Threading;
using System.Threading.Tasks;

namespace Better.Extensions.Runtime.Helpers
{
    internal abstract class BaseCompletionAwaiter<T>
    {
        private readonly TaskCompletionSource<T> _completionSource;
        public Task<T> Task => _completionSource.Task;

        public BaseCompletionAwaiter(CancellationToken cancellationToken)
        {
            _completionSource = new();
            cancellationToken.Register(Cancel);

            if (cancellationToken.IsCancellationRequested)
            {
                SetResult(default);
            }
        }

        protected void SetResult(T value)
        {
            if (!_completionSource.TrySetResult(value))
            {
                OnCompleted(value);
            }
        }

        private void Cancel()
        {
            SetResult(default);
        }

        protected abstract void OnCompleted(T result);
    }
}