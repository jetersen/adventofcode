namespace AdventOfCode.Year2020
{
    using Lib;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class Day05 : BaseDay
    {
        private List<int> _input = new();

        public Day05(IAdventClient client) : base(client)
        {
        }

        public override async Task LoadInput() => _input = (await File.ReadAllLinesAsync(InputFilePath))
            .Select(ParseSeatNumber)
            .ToList();

        public override string Solve_1() => _input.Max().ToString();

        public override string Solve_2() => Enumerable
            .Range(0, _input.Max())
            .Except(_input)
            .Max()
            .ToString();

        private static int ParseSeatNumber(string pass) =>
            Convert.ToInt32(ConvertToBinary(pass), 2);

        private static string ConvertToBinary(string pass) => pass
            .Replace('F', '0')
            .Replace('B', '1')
            .Replace('L', '0')
            .Replace('R', '1');
    }
}
