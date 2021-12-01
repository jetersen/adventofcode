namespace AdventOfCode.Lib;

public abstract class BaseDay : BaseProblem
{
    private readonly IAdventClient _client;

    protected BaseDay(IAdventClient client, IEnvironment environment, IFileSystem fileSystem) : base(environment, fileSystem)
    {
        _client = client;
        var type = GetType();
        Day = CalculateIndex(type);
        var ns = type.Namespace ?? string.Empty;
        Year = uint.Parse(ns[^4..]);
        InputFileDirPath = base.InputFileDirPath.Combine(new DirectoryPath(Year.ToString()));
    }

    protected override string ClassPrefix { get; } = "Day";

    public override Task FetchInput() => _client.FetchInput(Year, Day, InputFilePath);

    protected override DirectoryPath InputFileDirPath { get; }

    public uint Day { get; }
    public uint Year { get; }
}
