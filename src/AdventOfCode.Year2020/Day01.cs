namespace AdventOfCode.Year2020
{
    using Lib;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class Day01 : BaseDay
    {
        private List<int> _input = new();

        public Day01(AdventClient client) : base(client)
        {
        }

        public override async Task LoadInput() => _input = (await File.ReadAllLinesAsync(InputFilePath)).Select(int.Parse).ToList();

        public override string Solve_1() => _input
            .SelectMany(_ => _input, (x, y) => new {x, y})
            .Where(t => t.x + t.y == 2020)
            .Select(t => t.x * t.y).First().ToString();

        public override string Solve_2() => Part2_FastLinq();

        internal string Part2_FastLinq() => _input
            .Where(input1 =>
                _input.Find(input2 => _input.Contains(2020 - input1 - input2)) != default)
            .Aggregate(1, (o, c) => o * c)
            .ToString();

        internal string Part2_SlowLinq() => _input
            .SelectMany(x => _input, (x, y) => new {x, y})
            .SelectMany(t => _input, (t, z) => new {t.x, t.y, z})
            .Where(t => t.x + t.y + t.z == 2020)
            .Select(t => t.x * t.y * t.z).First().ToString();
    }
}
