namespace AdventOfCode.Lib;

public abstract class BaseProblem
{
    protected virtual string ClassPrefix { get; } = "Problem";

    /// <summary>
    /// Expected input file dir path.
    /// </summary>
    private string InputFileDirPath { get; } = "Inputs";

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
    /// Supported formats: <see cref="ClassPrefix"/>{Index}, <see cref="ClassPrefix"/>_{Index}.
    /// In case of unsupported class name format, <see cref="InputFilePath"/> needs to be overriden to point to the right input file.
    /// </summary>
    /// <returns>Problem's index or uint.MaxValue if unsupported class name.</returns>
    public virtual uint CalculateIndex()
    {
        var typeName = GetType().Name;
        var number = typeName
            .Substring(typeName.IndexOf(ClassPrefix, StringComparison.InvariantCulture) + ClassPrefix.Length)
            .TrimStart('_');

        return uint.TryParse(number, out var index)
            ? index
            : default;
    }

    /// <summary>
    /// Expected input file path.
    /// By default, <see cref="InputFileDirPath"/>/<see cref="CalculateIndex"/>.<see cref="InputFileExtension"/>.
    /// Overriding it makes <see cref="ClassPrefix"/>, <see cref="InputFileDirPath"/>, <see cref="InputFileExtension"/> and <see cref="CalculateIndex"/> irrelevant
    /// </summary>
    public virtual string InputFilePath
    {
        get
        {
            var index = CalculateIndex().ToString("D2");

            return Path.Combine(InputFileDirPath, $"{index}.{InputFileExtension.TrimStart('.')}");
        }
    }

    public abstract Task FetchInput();

    public abstract Task LoadInput();

    public abstract string Solve_1();

    public abstract string Solve_2();
}
