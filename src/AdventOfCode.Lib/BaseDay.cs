﻿using System.Threading.Tasks;

namespace AdventOfCode.Lib
{
    public abstract class BaseDay : BaseProblem
    {
        private readonly AdventClient _client;

        protected BaseDay(AdventClient client)
        {
            _client = client;
        }

        protected override string ClassPrefix { get; } = "Day";

        public override Task FetchInput() => _client.FetchInput(Year, Day, InputFilePath);

        public uint Day { get; set; }
        public uint Year { get; set; }

        public override uint CalculateIndex()
        {
            if (Day == 0) Day = base.CalculateIndex();
            if (Year != 0) return Day;

            var ns = GetType().Namespace ?? string.Empty;
            Year = uint.Parse(ns.Substring(ns.Length - 4));

            return Day;
        }
    }
}
