using System;

namespace AdventOfCode.Lib
{
    public static class Constants
    {
        public const string Endpoint = "https://adventofcode.com/";

        public static readonly string SessionCookie = Environment.GetEnvironmentVariable("AdventOfCodeSession") ?? "";
    }
}
