namespace AdventOfCode.Year2021;

public sealed class Day03 : BaseDay
{
    private string[] _input = Array.Empty<string>();

    public override async Task LoadInput() => _input = await File.ReadAllLinesAsync(InputFilePath.FullPath);

    public override ValueTask<string> Part1()
    {
        var size = _input[0].Length;
        var halfInputLength = _input.Length / 2;
        var gamma = 0;
        var epsilon = 0;
        for (var i = 0; i < size; i++)
        {
            var sum = _input.Count(number => number[i] == '1');
            if (sum >= halfInputLength)
            {
                epsilon <<= 1;
                gamma = (gamma << 1) + 1;
            }
            else
            {
                epsilon = (epsilon << 1) + 1;
                gamma <<= 1;
            }
        }

        return new($"{gamma * epsilon}");
    }

    public override ValueTask<string> Part2()
    {
        var oxygenGeneratorRating = FindRating(_input, true);
        var co2ScrubberRating = FindRating(_input, false);

        return new($"{oxygenGeneratorRating * co2ScrubberRating}");
    }

    private static int FindRating(IReadOnlyList<string> input, bool isMostCommon, int bitPosition = 0)
    {
        if (input.Count == 1)
        {
            return Convert.ToInt32(input[0], 2);
        }

        var sum = 2 * input.Count(number => number[bitPosition] == '1');

        var mostCommonChar = sum >= input.Count
            ? isMostCommon ? '1' : '0'
            : isMostCommon ? '0' : '1';

        return FindRating(input.Where(number => number[bitPosition] == mostCommonChar).ToArray(), isMostCommon, bitPosition + 1);
    }
}
