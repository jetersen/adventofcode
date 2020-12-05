namespace AdventOfCode.Year2020
{
    using Lib;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Tasks;

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
