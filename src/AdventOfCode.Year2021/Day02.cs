namespace AdventOfCode.Year2021;

public sealed class Day02 : BaseDay
{
    private string[] _input = Array.Empty<string>();

    public Day02(IAdventClient client, IEnvironment environment, IFileSystem fileSystem) : base(client, environment, fileSystem)
    {
    }

    public override async Task LoadInput() => _input = (await File.ReadAllLinesAsync(InputFilePath.FullPath));

    public override ValueTask<string> Solve_1()
    {
        var position = 0;
        var depth = 0;

        foreach (var instruction in _input)
        {
            var direction = instruction[0];
            var distance = int.Parse(instruction[^1..]);

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

        return new((position * depth).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var position = 0;
        var depth = 0;
        var aim = 0;

        foreach (var instruction in _input)
        {
            var direction = instruction[0];
            var distance = int.Parse(instruction[^1..]);

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

        return new((position * depth).ToString());
    }
}
