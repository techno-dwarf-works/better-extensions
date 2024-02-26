using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Better.Extensions.Runtime
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            return self.OrderBy(e => Guid.NewGuid());
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> self, Func<T, TKey> keySelector)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                yield break;
            }

            if (keySelector == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(keySelector));
                yield break;
            }

            var seenKeys = new HashSet<TKey>();
            foreach (var element in self)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static bool IsEmpty<T>(this IEnumerable<T> self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return true;
            }

            return !self.Any();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> self)
        {
            return self == null || self.IsEmpty();
        }

        public static ulong Sum<T>(this IEnumerable<T> self, Func<T, ulong> selector)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return default;
            }

            if (selector == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(selector));
                return default;
            }

            var sum = 0ul;
            foreach (var element in self)
            {
                sum += selector.Invoke(element);
            }

            return sum;
        }

        public static T GetRandom<T>(this IEnumerable<T> self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return default;
            }

            var valuesArray = self.ToArray();
            var index = Random.Range(0, valuesArray.Length);
            return valuesArray.ElementAt(index);
        }

        public static T GetRandom<T>(this IEnumerable<T> self, params T[] excludes)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return default;
            }

            self = self.Except(excludes);
            return self.GetRandom();
        }

        public static T GetRandomWithWeights<T>(this IEnumerable<T> self, Func<T, float> weightSelector)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return default;
            }

            if (weightSelector == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(weightSelector));
                return default;
            }

            float weight;
            var weightsValues = self.Select(value =>
            {
                weight = weightSelector.Invoke(value);
                return new Tuple<T, float>(value, weight);
            });

            return GetRandomWithWeights(weightsValues);
        }

        private static T GetRandomWithWeights<T>(this IEnumerable<Tuple<T, float>> self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return default;
            }

            var valuesArray = self.ToArray();
            if (valuesArray.IsEmpty())
            {
                var message = $"{nameof(valuesArray)} cannot be empty";
                DebugUtility.LogException<InvalidOperationException>(message);
                return default;
            }

            var totalWeight = valuesArray.Sum(v => v.Item2);
            if (totalWeight <= 0)
            {
                var message = $"[${nameof(EnumerableExtensions)}] {nameof(GetRandomWithWeights)}: Total weight is {totalWeight}, returned first item";
                Debug.LogWarning(message);
                return valuesArray[0].Item1;
            }

            var cumulativeWeight = Random.Range(0f, totalWeight);
            for (int i = 0; i < valuesArray.Length; i++)
            {
                cumulativeWeight -= valuesArray[i].Item2;
                if (cumulativeWeight <= 0)
                {
                    return valuesArray[i].Item1;
                }
            }

            var operationMessage = "Unexpected error occurred while selecting a weighted random item, returned first item";
            DebugUtility.LogException<InvalidOperationException>(operationMessage);
            return valuesArray[0].Item1;
        }

        public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> self, int count)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return Enumerable.Empty<T>();
            }

            if (count < 0)
            {
                var message = $"{nameof(count)} cannot be less 0";
                DebugUtility.LogException<ArgumentOutOfRangeException>(message);
                return Enumerable.Empty<T>();
            }

            var valuesList = self.ToList();
            valuesList.Shuffle();

            if (count >= valuesList.Count)
            {
                return valuesList;
            }

            return valuesList.Take(count);
        }

        public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> self, int count, params T[] excludes)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return Enumerable.Empty<T>();
            }

            self = self.Except(excludes);
            return self.GetRandom(count);
        }
    }
}