using System.Threading;
using System.Threading.Tasks;

namespace Better.Extensions.Runtime
{
    public static class TaskUtility
    {
        public static Task WaitForSeconds(float seconds, CancellationToken cancellationToken = default)
        {
            var millisecondsDelay = TimeUtility.SecondsToMilliseconds(seconds);
            return Task.Delay(millisecondsDelay, cancellationToken);
        }
    }
}