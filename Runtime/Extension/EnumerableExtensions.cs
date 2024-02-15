using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Better.Extensions.Runtime
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(e => Guid.NewGuid());
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        
        public static bool IsEmpty<T>(this IEnumerable<T> values)
        {
            return !values.Any();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> values)
        {
            return values == null || values.IsEmpty();
        }

        public static ulong Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, ulong> selector)
        {
            var sum = 0ul;
            foreach (var iterated in source)
            {
                sum += selector.Invoke(iterated);
            }

            return sum;
        }
        
        public static T GetRandom<T>(this IEnumerable<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(
                    $"[${nameof(EnumerableExtensions)}] {nameof(GetRandom)}: {nameof(values)} cannot be Empty or Null"
                );
            }

            var valuesArray = values.ToArray();
            var index = Random.Range(0, valuesArray.Length);
            return valuesArray.ElementAt(index);
        }

        public static T GetRandomWithWeights<T>(this IEnumerable<T> values, Func<T, float> weightSelector)
        {
            float weight;
            var weightsValues = values.Select(value =>
            {
                weight = weightSelector.Invoke(value);
                return new Tuple<T, float>(value, weight);
            });

            return GetRandomWithWeights(weightsValues);
        }

        private static T GetRandomWithWeights<T>(this IEnumerable<Tuple<T, float>> values)
        {
            if (values.IsNullOrEmpty())
            {
                throw new ArgumentNullException(
                    $"[${nameof(EnumerableExtensions)}] {nameof(GetRandomWithWeights)}: {nameof(values)} cannot be Empty or Null"
                );
            }

            var valuesArray = values.ToArray();
            var totalWeight = valuesArray.Sum(v => v.Item2);

            if (totalWeight <= 0)
            {
                Debug.LogWarning($"[${nameof(EnumerableExtensions)}] {nameof(GetRandomWithWeights)}: Total weight is {totalWeight}, returned first item");
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

            throw new InvalidOperationException(
                $"[${nameof(EnumerableExtensions)}] {nameof(GetRandomWithWeights)}: Unexpected error occurred while selecting a weighted random item"
            );
        }

        public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> values, int count)
        {
            count = Math.Max(count, 0);
            var valuesList = values.ToList();

            if (count >= valuesList.Count)
            {
                valuesList.Shuffle();
                return valuesList;
            }

            var selectedItems = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                var index = Random.Range(0, valuesList.Count);
                selectedItems.Add(valuesList[index]);
                valuesList.RemoveAt(index);
            }

            return selectedItems;
        }

        public static T GetRandom<T>(this IEnumerable<T> values, params T[] excludes)
        {
            values = values.Except(excludes);
            return values.GetRandom();
        }
    }
}