namespace AdventOfCode.Year2020.Tests;

public class UnitTests : IClassFixture<DayFixture>
{
    private readonly DayFixture _fixture;

    public UnitTests(DayFixture dayFixture)
    {
        _fixture = dayFixture;
    }

    [Theory]
    [InlineData(typeof(Day01), "100419")]
    [InlineData(typeof(Day02), "398")]
    [InlineData(typeof(Day03), "207")]
    [InlineData(typeof(Day04), "228")]
    public async Task Part1(Type type, string expected)
    {
        var actual = await _fixture.Days.First(x => x.GetType() == type).Part1();
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(typeof(Day01), "265253940")]
    [InlineData(typeof(Day02), "562")]
    [InlineData(typeof(Day03), "2655892800")]
    [InlineData(typeof(Day04), "175")]
    public async Task Part2(Type type, string expected)
    {
        var actual = await _fixture.Days.First(x => x.GetType() == type).Part2();
        actual.Should().Be(expected);
    }
}
