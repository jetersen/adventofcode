namespace AdventOfCode.Lib;

public abstract class BaseProblem
{
    private static readonly char[] Digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    private readonly IEnvironment _environment;
    private readonly IFileSystem _fileSystem;

    protected BaseProblem(IEnvironment environment, IFileSystem fileSystem)
    {
        _environment = environment;
        _fileSystem = fileSystem;
        var directoryPath = new DirectoryPath("Inputs");
        directoryPath.MakeAbsolute(environment);
        InputFileDirPath = directoryPath;
        var index = CalculateIndex(GetType());
        Index = index;
        var filePath = directoryPath.CombineWithFilePath(new FilePath($"{index:D2}.{InputFileExtension.TrimStart('.')}"));
        InputFilePath = filePath;
    }

    /// <summary>
    /// Expected input file dir path.
    /// </summary>
    protected virtual DirectoryPath InputFileDirPath { get; } = new("Inputs");

    /// <summary>
    /// Expected input file extension.
    /// </summary>
    private string InputFileExtension { get; } = ".txt";

    /// <summary>
    /// Problem's index.
    /// Two digit number, (expect a leading '0' when appropriated).
    /// </summary>
    /// <summary>
    /// Extracts problem's index from the class name.
    /// In case of unsupported class name format, <see cref="InputFilePath"/> needs to be overriden to point to the right input file.
    /// </summary>
    /// <returns>Problem's index or uint.MaxValue if unsupported class name.</returns>
    protected static uint CalculateIndex(Type type)
    {
        var typeName = type.Name;
        var numberIndex = typeName.IndexOfAny(Digits);
        var number = typeName[numberIndex..];

        return uint.TryParse(number, out var index)
            ? index
            : default;
    }

    /// <summary>
    /// Expected input file path.
    /// By default, <see cref="InputFileDirPath"/>/<see cref="CalculateIndex"/>.<see cref="InputFileExtension"/>.
    /// Overriding it makes <see cref="InputFileDirPath"/>, <see cref="InputFileExtension"/> and <see cref="CalculateIndex"/> irrelevant
    /// </summary>
    public virtual FilePath InputFilePath { get; }
    public virtual uint Index { get; }

    public abstract Task FetchInput();

    public abstract Task LoadInput();

    public abstract ValueTask<string> Solve_1();

    public abstract ValueTask<string> Solve_2();
}
