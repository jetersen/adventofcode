namespace AdventOfCode.Lib;

public static class Constants
{
    private const string CookieName = "AdventOfCodeSession";
    public const string Endpoint = "https://adventofcode.com/";
    public static readonly string SessionCookie = Environment.GetEnvironmentVariable(CookieName) ?? "";
}
