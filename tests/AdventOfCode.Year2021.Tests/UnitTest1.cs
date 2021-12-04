namespace AdventOfCode.Year2021.Tests;

public class UnitTest1
{
    [Theory]
    [InlineData(typeof(Day01), "1696", "1737")]
    [InlineData(typeof(Day02), "1451208", "1620141160")]
    [InlineData(typeof(Day03), "1307354", "482500")]
    [InlineData(typeof(Day04), "72770", "13912")]
    public async Task Test1(Type type, string part1, string part2)
    {
        if (Activator.CreateInstance(type) is BaseDay instance)
        {
            await instance.LoadInput();
            Assert.Equal(part1, await instance.Solve_1());
            Assert.Equal(part2, await instance.Solve_2());
        }
        else
        {
            Assert.True(false, "Failed to create instance of type");
        }
    }
}
