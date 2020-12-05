namespace AdventOfCode.Lib
{
    using System;

    public static class Constants
    {
        public const string Endpoint = "https://adventofcode.com/";

        public static readonly string SessionCookie = Environment.GetEnvironmentVariable("AdventOfCodeSession") ?? "";
    }
}
