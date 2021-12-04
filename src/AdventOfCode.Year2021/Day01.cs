namespace AdventOfCode.Year2021;

public sealed class Day01 : BaseDay
{
    private int[] _input = Array.Empty<int>();

    public override async Task LoadInput() => _input = (await File.ReadAllLinesAsync(InputFilePath.FullPath)).Select(int.Parse).ToArray();

    public override ValueTask<string> Solve_1()
    {
        return Solve(1);
    }

    public override ValueTask<string> Solve_2()
    {
        return Solve(3);
    }

    private ValueTask<string> Solve(int slideSize)
    {
        var result = 0;
        for (int i = slideSize; i < _input.Length; ++i)
        {
            if (_input[i] > _input[i - slideSize])
            {
                ++result;
            }
        }

        return new($"{result}");
    }
}
