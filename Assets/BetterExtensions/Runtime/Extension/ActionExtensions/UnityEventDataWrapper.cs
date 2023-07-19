using System.Threading.Tasks;
using UnityEngine.Events;

namespace Better.Extensions.Runtime.ActionExtensions
{
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
}