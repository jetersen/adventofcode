namespace AdventOfCode.Year2021;

public sealed class Day02 : BaseDay
{
    private DirectionDistance[] _input = Array.Empty<DirectionDistance>();

    private record DirectionDistance(char Direction, int Distance);

    public override async Task LoadInput() => _input = (await File.ReadAllLinesAsync(InputFilePath.FullPath)).Select(ParseInput).ToArray();

    private static DirectionDistance ParseInput(string arg)
    {
        return new(arg[0], int.Parse(arg[^1..]));
    }

    public override ValueTask<string> Part1()
    {
        var position = 0;
        var depth = 0;

        foreach (var (direction, distance) in _input)
        {
            switch (direction)
            {
                case 'f':
                    position += distance;
                    break;
                case 'd':
                    depth += distance;
                    break;
                case 'u':
                    depth -= distance;
                    break;
            }
        }

        return new($"{position * depth}");
    }

    public override ValueTask<string> Part2()
    {
        var position = 0;
        var depth = 0;
        var aim = 0;

        foreach (var (direction, distance) in _input)
        {
            switch (direction)
            {
                case 'f':
                    position += distance;
                    depth += aim * distance;
                    break;
                case 'd':
                    aim += distance;
                    break;
                case 'u':
                    aim -= distance;
                    break;
            }
        }

        return new($"{position * depth}");
    }
}
