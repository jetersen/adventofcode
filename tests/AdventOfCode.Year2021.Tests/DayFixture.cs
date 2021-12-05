namespace AdventOfCode.Year2021.Tests;

public class DayFixture : IAsyncLifetime
{
    public BaseDay[] Days { get; }

    public DayFixture()
    {
        Days = new BaseDay[]
        {
            new Day01(),
            new Day02(),
            new Day03(),
            new Day04(),
        };
    }

    public async Task InitializeAsync()
    {
        await Task.WhenAll(Days.Select(day => day.LoadInput()));
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
