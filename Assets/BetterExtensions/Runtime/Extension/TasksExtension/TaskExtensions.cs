using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Better.Extensions.Runtime.TasksExtension
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Blocks while condition is true or timeout occurs.
        /// </summary>
        /// <param name="condition">The condition that will perpetuate the block.</param>
        /// <param name="frequency">The frequency at which the condition will be check, in milliseconds.</param>
        /// <param name="timeout">Timeout in milliseconds.</param>
        /// <exception cref="TimeoutException"></exception>
        /// <returns></returns>
        public static async Task WaitWhile(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (condition())
                    await Task.Delay(frequency);
            });
            if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
                throw new TimeoutException();
        }

        /// <summary>
        /// Blocks until condition is true or timeout occurs.
        /// </summary>
        /// <param name="condition">The break condition.</param>
        /// <param name="frequency">The frequency at which the condition will be checked.</param>
        /// <param name="timeout">The timeout in milliseconds.</param>
        /// <returns></returns>
        public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!condition())
                    await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
                throw new TimeoutException();
        }

        public static async void Forget(this Task task)
        {
            await task;
        }
        
        /// <summary>
        /// Creates task with factory method
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<T> CreateTask<T>(this Func<T> action)
        {
            return Task<T>.Factory.StartNew(action);
        }

        public static Task WhenAll(this IEnumerable<Task> tasks)
        {
            return Task.WhenAll(tasks);
        }
        
        public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> tasks)
        {
            return Task.WhenAll(tasks);
        }

        public static Task WhenAny(this IEnumerable<Task> tasks)
        {
            return Task.WhenAny(tasks);
        }
        
        public static async Task<T> WhenAny<T>(this IEnumerable<Task<T>> tasks)
        {
            var task = await Task.WhenAny(tasks);
            return await task;
        }
        
        public static Task WaitForSeconds(float seconds, CancellationToken cancellationToken = default)
        {
            return Task.Delay(Mathf.RoundToInt(seconds * 1000), cancellationToken);
        }
    }
}