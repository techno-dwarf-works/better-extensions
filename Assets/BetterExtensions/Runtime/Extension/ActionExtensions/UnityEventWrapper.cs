using System.Threading.Tasks;
using UnityEngine.Events;

namespace Better.Extensions.Runtime.ActionExtensions
{
    internal class UnityEventWrapper<T> : BaseActionWrapper<T>
    {
        private UnityEvent<T> _action;

        public UnityEventWrapper(TaskCompletionSource<T> taskCompletionSource, ref UnityEvent<T> action) : base(
            taskCompletionSource)
        {
            _action = action;
        }

        public override Task<T> Initialize()
        {
            _action.AddListener(SetResult);
            return _taskCompletionSource.Task;
        }

        public override void Cancel()
        {
            _action.RemoveListener(SetResult);
            _taskCompletionSource.TrySetCanceled();
        }

        private void SetResult(T result)
        {
            _action.RemoveListener(SetResult);
            _taskCompletionSource.TrySetResult(result);
        }
    }
    
    internal class UnityEventDataWrapper<T> : BaseActionWrapper<T>
    {
        private UnityEvent _action;
        private readonly T _data;

        public UnityEventDataWrapper(TaskCompletionSource<T> taskCompletionSource, ref UnityEvent action, T data) : base(
            taskCompletionSource)
        {
            _action = action;
            _data = data;
        }

        public override Task<T> Initialize()
        {
            _action.AddListener(SetResult);
            return _taskCompletionSource.Task;
        }

        public override void Cancel()
        {
            _action.RemoveListener(SetResult);
            _taskCompletionSource.TrySetCanceled();
        }

        private void SetResult()
        {
            _action.RemoveListener(SetResult);
            _taskCompletionSource.TrySetResult(_data);
        }
    }
    
    internal class UnityEventWrapper : BaseActionWrapper<bool>
    {
        private UnityEvent _action;

        public UnityEventWrapper(TaskCompletionSource<bool> taskCompletionSource, ref UnityEvent action) : base(
            taskCompletionSource)
        {
            _action = action;
        }

        public override Task<bool> Initialize()
        {
            _action.AddListener(SetResult);
            return _taskCompletionSource.Task;
        }

        private void SetResult()
        {
            _action.RemoveListener(SetResult);
            _taskCompletionSource.TrySetResult(true);
        }

        public override void Cancel()
        {
            _action.RemoveListener(SetResult);
            _taskCompletionSource.TrySetCanceled();
        }
    }
}