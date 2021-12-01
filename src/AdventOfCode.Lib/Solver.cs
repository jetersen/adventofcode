namespace AdventOfCode.Lib;

/// <summary>
/// Derivative work of https://github.com/eduherminio/AoCHelper
/// The main difference is Solver remains non static and allows for dependency injection and is asynchronous.
/// One is expected to call the Render methods to get the output which is where BaseProgram helps.
/// </summary>
public class Solver
{
    private readonly IEnumerable<BaseProblem> _baseProblems;
    private readonly List<Row> _rows = new();
    private readonly List<(double part1, double part2)> _totalElapsedTime = new();
    private readonly List<double> _totalElapsedLoadTime = new();

    public Solver(IEnumerable<BaseProblem> baseProblems)
    {
        _baseProblems = baseProblems;
    }

    /// <summary>
    /// Numeric format strings, see https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
    /// </summary>
    public static string? ElapsedTimeFormatSpecifier { get; set; } = null;

    private static readonly bool IsInteractiveEnvironment = System.Environment.UserInteractive && !Console.IsOutputRedirected;

    private static Table GetTable() => new Table()
        .AddColumns("[bold white]Day[/]", "[bold white]Part[/]", "[bold white]Solution[/]", "[bold white]Elapsed time[/]")
        .RoundedBorder()
        .BorderColor(Color.Grey);

    #region Public methods

    /// <summary>
    /// Solves a problem.
    /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
    /// </summary>
    /// <typeparam name="TProblem"></typeparam>
    public Task Solve<TProblem>()
        where TProblem : BaseProblem, new() =>
        SolveProblem(new TProblem());

    /// <summary>
    /// Solves those problems whose <see cref="BaseProblem.CalculateIndex"/> method matches one of the provided numbers.
    /// 0 can be used for those problems whose <see cref="BaseProblem.CalculateIndex"/> returns the default value due to not being able to deduct the index.
    /// </summary>
    /// <param name="problemNumbers"></param>
    public IEnumerable<Task> Solve(params uint[] problemNumbers) => Solve(problemNumbers.AsEnumerable());

    /// <summary>
    /// Solves those problems whose <see cref="BaseProblem.CalculateIndex"/> method matches one of the provided numbers.
    /// 0 can be used for those problems whose <see cref="BaseProblem.CalculateIndex"/> returns the default value due to not being able to deduct the index.
    /// </summary>
    /// <param name="problemNumbers"></param>
    public IEnumerable<Task> Solve(IEnumerable<uint> problemNumbers) =>
        _baseProblems
            .Where(problem => problemNumbers.Contains(problem.Index))
            .Select(SolveProblem);

    /// <summary>
    /// Solves last problem.
    /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
    /// </summary>
    /// <param name="clearConsole"></param>
    public Task SolveLast(bool clearConsole = true) => SolveProblem(_baseProblems.Last());

    /// <summary>
    /// Solves the provided problems.
    /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
    /// </summary>
    public IEnumerable<Task> Solve(params Type[] problems) => Solve(problems.AsEnumerable());

    /// <summary>
    /// Solves the provided problems.
    /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
    /// </summary>
    public IEnumerable<Task> Solve(IEnumerable<Type> problems) => _baseProblems.Select(SolveProblem);

    /// <summary>
    /// Solves all problems in the assembly.
    /// It also prints the elapsed time in <see cref="BaseProblem.Solve_1"/> and <see cref="BaseProblem.Solve_2"/> methods.
    /// </summary>
    public IEnumerable<Task> SolveAll() => _baseProblems.Select(SolveProblem);

    #endregion

    private async Task SolveProblem(BaseProblem problem)
    {
        var problemIndex = problem.Index;
        var problemTitle = problemIndex != default
            ? $"Day {problemIndex}"
            : $"{problem.GetType().Name}";

        await LoadInput(problem).ConfigureAwait(false);

        var (solution1, elapsedMillisecondsPart1) = SolvePart(true, problem);
        var (solution2, elapsedMillisecondsPart2) = SolvePart(false, problem);

        _rows.Add(new Row(problemTitle, 1, solution1, elapsedMillisecondsPart1));
        _rows.Add(new Row(problemTitle, 2, solution2, elapsedMillisecondsPart2));

        _totalElapsedTime.Add((elapsedMillisecondsPart1, elapsedMillisecondsPart2));
    }

    private static (string solution, double elapsedTime) SolvePart(bool isPart1, BaseProblem problem)
    {
        var stopwatch = new Stopwatch();
        string solution;

        try
        {
            Func<string> solve = isPart1
                ? problem.Solve_1
                : problem.Solve_2;

            stopwatch.Start();
            solution = solve();
        }
        catch (NotImplementedException)
        {
            solution = "[[Not implemented]]";
        }
        catch (Exception e)
        {
            solution = e.Message + System.Environment.NewLine + e.StackTrace;
        }
        finally
        {
            stopwatch.Stop();
        }

        var elapsedMilliseconds = CalculateElapsedMilliseconds(stopwatch);
        return (solution, elapsedMilliseconds);
    }

    private async Task LoadInput(BaseProblem problem)
    {
        var stopwatch = Stopwatch.StartNew();
        await problem.FetchInput().ConfigureAwait(false);
        await problem.LoadInput().ConfigureAwait(false);
        stopwatch.Stop();
        var elapsedMilliseconds = CalculateElapsedMilliseconds(stopwatch);
        _totalElapsedLoadTime.Add(elapsedMilliseconds);
    }

    /// <summary>
    /// http://geekswithblogs.net/BlackRabbitCoder/archive/2012/01/12/c.net-little-pitfalls-stopwatch-ticks-are-not-timespan-ticks.aspx
    /// </summary>
    /// <param name="stopwatch"></param>
    /// <returns></returns>
    private static double CalculateElapsedMilliseconds(Stopwatch stopwatch) =>
        1000 * stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;

    private static string FormatTime(double elapsedMilliseconds, bool useColor = true)
    {
        var message = ElapsedTimeFormatSpecifier is null
            ? elapsedMilliseconds switch
            {
                < 1 => $"{elapsedMilliseconds:F} ms",
                < 1_000 => $"{Math.Round(elapsedMilliseconds)} ms",
                < 60_000 => $"{0.001 * elapsedMilliseconds:F} s",
                _ => $"{elapsedMilliseconds / 60_000} min {Math.Round(0.001 * (elapsedMilliseconds % 60_000))} s",
            }
            : elapsedMilliseconds switch
            {
                < 1 => $"{elapsedMilliseconds.ToString(ElapsedTimeFormatSpecifier)} ms",
                < 1_000 => $"{elapsedMilliseconds.ToString(ElapsedTimeFormatSpecifier)} ms",
                < 60_000 => $"{(0.001 * elapsedMilliseconds).ToString(ElapsedTimeFormatSpecifier)} s",
                _ =>
                    $"{elapsedMilliseconds / 60_000} min {(0.001 * (elapsedMilliseconds % 60_000)).ToString(ElapsedTimeFormatSpecifier)} s",
            };

        if (useColor == false) return message;

        Color color = elapsedMilliseconds switch
        {
            < 1 => Color.Blue,
            < 10 => Color.Green1,
            < 100 => Color.Lime,
            < 500 => Color.GreenYellow,
            < 1_000 => Color.Yellow1,
            < 10_000 => Color.OrangeRed1,
            _ => Color.Red1,
        };

        return $"[{color}]{message}[/]";
    }

    public void Render(bool clearConsole)
    {
        var table = GetTable();

        var rows = _rows.OrderBy(r => r.Day).ThenBy(r => r.Part).ToList();

        var rowsCount = rows.Count;
        var lastRow = rowsCount - 1;
        for (var i = 0; i < rowsCount; i++)
        {
            var (day, part, solution, elapsedTime) = rows[i];
            table.AddRow(day, $"Part {part}", solution, FormatTime(elapsedTime));
            if (part % 2 == 0 && i != lastRow) table.AddEmptyRow();
        }

        if (IsInteractiveEnvironment)
        {
            if (clearConsole)
                Console.Clear();
            else
                AnsiConsole.Console.Clear(true);
        }

        AnsiConsole.Write(table);
    }

    public void RenderOverallResults()
    {
        var totalPart1 = _totalElapsedTime.Select(t => t.part1).Sum();
        var totalPart2 = _totalElapsedTime.Select(t => t.part2).Sum();
        var total = totalPart1 + totalPart2;

        var grid = new Grid()
            .AddColumn(new GridColumn().NoWrap().PadRight(4))
            .AddColumn()
            .AddRow()
            .AddRow($"Total ({_totalElapsedTime.Count} days)", FormatTime(total, false))
            .AddRow("Total parts 1", FormatTime(totalPart1, false))
            .AddRow("Total parts 2", FormatTime(totalPart2, false))
            .AddRow("Total load", FormatTime(_totalElapsedLoadTime.Sum(), false))
            .AddRow()
            .AddRow("Mean (per day)", FormatTime(total / _totalElapsedTime.Count))
            .AddRow("Mean parts 1", FormatTime(_totalElapsedTime.Select(t => t.part1).Average()))
            .AddRow("Mean parts 2", FormatTime(_totalElapsedTime.Select(t => t.part2).Average()))
            .AddRow("Mean load", FormatTime(_totalElapsedLoadTime.Sum() / _totalElapsedLoadTime.Count));

        AnsiConsole.Write(
            new Panel(grid)
                .Header("[b] Overall results [/]", Justify.Center));
    }
}

public record Row(string Day = "", int Part = 0, string Solution = "", double elapsedPartTime = 0);
