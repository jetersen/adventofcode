namespace AdventOfCode.Lib;

public abstract class BaseDay : BaseProblem
{
    protected BaseDay()
    {
        var type = GetType();
        Day = CalculateIndex(type);
        var ns = type.Namespace ?? string.Empty;
        Year = uint.Parse(ns[^4..]);
    }

    public override Task FetchInput(IAdventClient client) => client.FetchInput(Year, Day, InputFilePath);

    public uint Day { get; }
    public uint Year { get; }
}
