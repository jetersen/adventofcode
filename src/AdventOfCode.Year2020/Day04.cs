namespace AdventOfCode.Year2020;

public class Day04 : BaseDay
{
    private List<Dictionary<string, string>> _input = new();

    public Day04(IAdventClient client, IEnvironment environment, IFileSystem fileSystem) : base(client, environment, fileSystem)
    {
    }

    public override async Task LoadInput()
    {
        var input = await File.ReadAllTextAsync(InputFilePath.FullPath);
        IEnumerable<MatchCollection> passportKeyValueMatches = input
            .Split("\n\n")
            .Select(onePassport => KeyValueRegex.Matches(onePassport));


        _input = passportKeyValueMatches
            .Select(passportMatch => passportMatch.Select(match => (match.GetGroupValue("key"), match.GetGroupValue("value"))))
            .Select(enumerable => enumerable.ToDictionary(x => x.Item1, x => x.Item2))
            .ToList();
    }

    public override string Solve_1() => _input
        .Count(dict => ValidFields.All(key =>
            dict.TryGetValue(key, out var str)))
        .ToString();

    public override string Solve_2() => _input
        .Count(dict => ValidFields.All(key =>
            dict.TryGetValue(key, out var str) && RegexExpressions[key].IsMatch(str)))
        .ToString();

    private static readonly Regex KeyValueRegex = new(@"(?<key>\S*):(?<value>\S*)\s?");

    private static readonly string[] ValidFields = {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};

    /// <summary>
    /// Regex expressions by @robertosanval, https://github.com/robertosanval/aoc2020/blob/master/src/day4/index.js
    /// </summary>
    private static readonly Dictionary<string, Regex> RegexExpressions = new()
    {
        ["byr"] = new Regex(@"^(19[2-8][0-9]|199[0-9]|200[0-2])$"),
        ["iyr"] = new Regex(@"^(201[0-9]|2020)$"),
        ["eyr"] = new Regex(@"^(202[0-9]|2030)$"),
        ["hgt"] = new Regex(@"^((1[5-8][0-9]|19[0-3])cm|(59|6[0-9]|7[0-6])in)$"),
        ["hcl"] = new Regex(@"^#[0-9a-f]{6}$"),
        ["ecl"] = new Regex(@"^amb|blu|brn|gry|grn|hzl|oth$"),
        ["pid"] = new Regex(@"^\d{9}$"),
    };
}
