using System.Threading.Tasks;

namespace Better.Extensions.Runtime.Extension.ActionExtensions
{
    internal abstract class BaseActionWrapper<T>
    {
        private protected TaskCompletionSource<T> _taskCompletionSource;
    
        public BaseActionWrapper(TaskCompletionSource<T> taskCompletionSource)
        {
            _taskCompletionSource = taskCompletionSource;
        }
    
        public abstract Task<T> Initialize();


        public abstract void Cancel();
    }
}