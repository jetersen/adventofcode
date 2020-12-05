namespace AdventOfCode.Lib
{
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public abstract class BaseProgram
    {
        public static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddAdventOfCodeHttpClient();
            services.RegisterAllTypes<BaseProblem>(Assembly.GetEntryAssembly()!);
            services.AddSingleton<Solver>();
            return services;
        }

        public static ServiceProvider BuildServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        public static async Task RunSolver(string[] args, Solver solver)
        {
            var all = false;
            switch (args.Length)
            {
                // dotnet run
                case 0:
                    await solver.SolveLast();
                    break;
                // dotnet run all | dotnet run --all
                case 1 when args[0].Contains("all", StringComparison.CurrentCultureIgnoreCase):
                    all = true;
                    await Task.WhenAll(solver.SolveAll());
                    break;
                // dotnet run 1 2 5 10
                default:
                {
                    var indexes = args.Select(arg => uint.TryParse(arg, out var index) ? index : uint.MaxValue);
                    await Task.WhenAll(solver.Solve(indexes.Where(i => i < uint.MaxValue)));
                    break;
                }
            }

            solver.Render(false);
            if (all) solver.RenderOverallResults();
        }
    }
}
