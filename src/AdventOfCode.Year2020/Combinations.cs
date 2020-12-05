using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2020
{
    public static class Combinatorics
    {
        public static IEnumerable<int> Combinations(IEnumerable<int> list, int length, int target)
        {
            return Combinations(list, length).First(i => i.ToArray().Sum() == target);
        }

        public static IEnumerable<IEnumerable<T>> Combinations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new[] {t});
            var enumerable = list as T[] ?? list.ToArray();

            return Combinations(enumerable, length - 1)
                .SelectMany(t => enumerable, (t1, t2) => t1.Concat(new[] {t2}));
        }
    }
}
