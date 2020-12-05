using System;
using System.Reflection;
using System.Threading.Tasks;
using AdventOfCode.Lib;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode.Year2020
{
    public class Program : BaseProgram
    {
        public static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = BuildServiceProvider(services);
            var solver = serviceProvider.GetRequiredService<Solver>();

            await RunSolver(args, solver);
        }
    }
}
