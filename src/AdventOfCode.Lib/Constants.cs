namespace AdventOfCode.Lib;

public static class Constants
{
    private const string CookieName = "AdventOfCodeSession";
    public const string Endpoint = "https://adventofcode.com/";
    public static readonly string SessionCookie = EnvReader.TryGetStringValue(CookieName, out var sessionCookie) ? sessionCookie : "";
    public static readonly char[] Digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
}
