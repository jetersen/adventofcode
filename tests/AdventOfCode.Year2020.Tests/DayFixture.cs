namespace AdventOfCode.Year2020.Tests;

public class DayFixture : IAsyncLifetime
{
    public IEnumerable<BaseDay> Days { get; }

    public DayFixture()
    {
        Days = new BaseDay[]
        {
            new Day01(),
            new Day02(),
            new Day03(),
            new Day04(),
            new Day05(),
        };
    }

    public async Task InitializeAsync()
    {
        foreach (var baseDay in Days)
        {
            await baseDay.LoadInput();
        }
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
