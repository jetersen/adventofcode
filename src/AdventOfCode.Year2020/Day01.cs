namespace AdventOfCode.Year2020;

public sealed class Day01 : BaseDay
{
    private List<int> _input = new();

    public override async Task LoadInput() => _input = (await File.ReadAllLinesAsync(InputFilePath.FullPath)).Select(int.Parse).ToList();

    public override ValueTask<string> Solve_1() => new(Part1_Linq());

    public override ValueTask<string> Solve_2() => new(Part2_FastLinq());

    internal string Part1_Linq() => _input
        .SelectMany(_ => _input, (x, y) => new { x, y })
        .Where(t => t.x + t.y == 2020)
        .Select(t => t.x * t.y).First().ToString();

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
