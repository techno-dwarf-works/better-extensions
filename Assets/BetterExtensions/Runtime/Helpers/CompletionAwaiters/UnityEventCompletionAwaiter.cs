using System.Threading;
using UnityEngine.Events;

namespace Better.Extensions.Runtime.Helpers
{
    internal class UnityEventCompletionAwaiter<T> : BaseCompletionAwaiter<T>
    {
        private UnityEvent<T> _sourceEvent;

        public UnityEventCompletionAwaiter(UnityEvent<T> sourceEvent, CancellationToken cancellationToken)
            : base(cancellationToken)
        {
            _sourceEvent = sourceEvent;
            _sourceEvent.AddListener(OnSourceInvoked);
        }

        private void OnSourceInvoked(T value) => SetResult(value);
        protected override void OnCompleted(T result) => _sourceEvent.RemoveListener(OnSourceInvoked);
    }

    internal class UnityEventCompletionAwaiter : BaseCompletionAwaiter<bool>
    {
        private UnityEvent _sourceEvent;

        public UnityEventCompletionAwaiter(UnityEvent sourceEvent, CancellationToken cancellationToken)
            : base(cancellationToken)
        {
            _sourceEvent = sourceEvent;
            _sourceEvent.AddListener(OnSourceInvoked);
        }

        private void OnSourceInvoked() => SetResult(true);
        protected override void OnCompleted(bool result) => _sourceEvent.RemoveListener(OnSourceInvoked);
    }
}