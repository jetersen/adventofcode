﻿namespace AdventOfCode.Year2020;

public class Day05 : BaseDay
{
    private List<int> _input = new();

    public override async Task LoadInput() => _input = (await File.ReadAllLinesAsync(InputFilePath.FullPath))
        .Select(ParseSeatNumber)
        .ToList();

    public override ValueTask<string> Part1() => new(_input.Max().ToString());

    public override ValueTask<string> Part2() => new(Enumerable
        .Range(0, _input.Max())
        .Except(_input)
        .Max()
        .ToString());

    private static int ParseSeatNumber(string pass) =>
        Convert.ToInt32(ConvertToBinary(pass), 2);

    private static string ConvertToBinary(string pass) => pass
        .Replace('F', '0')
        .Replace('B', '1')
        .Replace('L', '0')
        .Replace('R', '1');
}
