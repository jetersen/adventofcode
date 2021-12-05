namespace AdventOfCode.Year2021.Tests;

public class UnitTests : IClassFixture<DayFixture>
{
    private readonly DayFixture _fixture;

    public UnitTests(DayFixture dayFixture)
    {
        _fixture = dayFixture;
    }

    [Theory]
    [InlineData(typeof(Day01), "1696")]
    [InlineData(typeof(Day02), "1451208")]
    [InlineData(typeof(Day03), "1307354")]
    [InlineData(typeof(Day04), "72770")]
    public async Task Part1(Type type, string expected)
    {
        var actual = await _fixture.Days.First(x => x.GetType() == type).Part1();
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(typeof(Day01), "1737")]
    [InlineData(typeof(Day02), "1620141160")]
    [InlineData(typeof(Day03), "482500")]
    [InlineData(typeof(Day04), "13912")]
    public async Task Part2(Type type, string expected)
    {
        var actual = await _fixture.Days.First(x => x.GetType() == type).Part2();
        actual.Should().Be(expected);
    }
}
