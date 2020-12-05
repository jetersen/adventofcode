namespace AdventOfCode.Year2020
{
    using Lib;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class Day03 : BaseDay
    {
        private List<string> _input = new();

        public Day03(AdventClient client) : base(client)
        {
        }

        public override async Task LoadInput()
        {
            _input = (await File.ReadAllLinesAsync(InputFilePath)).ToList();
        }

        public override string Solve_1() => TransverseMap(
                new[] {(x: 3, y: 1)},
                ch => ch == '#')
            .Single().ToString();

        public override string Solve_2() => TransverseMap(
                new[] {(x: 1, y: 1), (x: 3, y: 1), (x: 5, y: 1), (x: 7, y: 1), (x: 1, y: 2),},
                ch => ch == '#')
            .Aggregate(1L, (total, n) => total * n)
            .ToString();

        internal IEnumerable<int> TransverseMap(ICollection<(int x, int y)> slopes, Func<char, bool> predicate)
        {
            var matches = new Dictionary<(int x, int y), int>(
                slopes.Select(slope => new KeyValuePair<(int x, int y), int>(slope, 0)));

            for (var level = 1; level < _input.Count; ++level)
            {
                var mapLine = _input[level];
                foreach (var slope in slopes)
                {
                    if (level % slope.y != 0) continue;
                    var y = level / slope.y;
                    var x = (y * slope.x) % mapLine.Length;

                    if (!predicate.Invoke(mapLine[x])) continue;
                    ++matches[slope];
                }
            }

            return matches.Select(t => t.Value);
        }
    }
}
