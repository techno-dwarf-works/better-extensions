using System.Threading.Tasks;

namespace BetterExtensions.Runtime.Extension.ActionExtensions
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