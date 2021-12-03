namespace AdventOfCode.Year2020;

public sealed class Day02 : BaseDay
{
    private List<string> _input = new();

    private static readonly Regex Regex = new(@"(?<min>\d+)-(?<max>\d+) (?<character>\w): (?<password>\w+)");

    public Day02(IEnvironment environment, IFileSystem fileSystem) : base(environment, fileSystem)
    {
    }

    public override async Task LoadInput() => _input = (await File.ReadAllLinesAsync(InputFilePath.FullPath)).ToList();

    public static (string password, PasswordPolicy policy) ParseInput(string input)
    {
        Match? match = Regex.Match(input);

        return (
            match.GetGroupValue("password"),
            new PasswordPolicy(
                match.GetGroupValue<char>("character"),
                match.GetGroupValue<int>("min"),
                match.GetGroupValue<int>("max")
            ));
    }

    public override ValueTask<string> Solve_1() => new(_input
        .Select(ParseInput)
        .Count(x => IsPasswordValid(x.password, x.policy))
        .ToString());

    public override ValueTask<string> Solve_2() => new(_input
        .Select(ParseInput)
        .Count(x => IsPasswordValid2(x.password, x.policy))
        .ToString());

    private static bool IsPasswordValid(string password, PasswordPolicy policy)
    {
        var policyCharCount = password.ToCharArray().Count(c => c == policy.Char);
        return policyCharCount >= policy.Min && policyCharCount <= policy.Max;
    }

    private static bool IsPasswordValid2(string password, PasswordPolicy policy) =>
        (password[policy.Min - 1] == policy.Char) ^ (password[policy.Max - 1] == policy.Char);
}

public record PasswordPolicy(char Char, int Min, int Max);
