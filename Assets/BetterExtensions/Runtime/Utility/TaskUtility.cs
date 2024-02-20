using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Better.Extensions.Runtime.Utility
{
    public static class TaskUtility
    {
        public static Task WaitForSeconds(float seconds, CancellationToken cancellationToken = default)
        {
            return Task.Delay(Mathf.RoundToInt(seconds * 1000), cancellationToken);
        }
    }
}