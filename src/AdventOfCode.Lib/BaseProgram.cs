namespace AdventOfCode.Lib;

public static class BaseProgram
{
    public static IServiceCollection ConfigureServices(IServiceCollection services)
    {
        services.AddAdventOfCodeHttpClient();
        services.AddSingleton<IEnvironment, Spectre.IO.Environment>();
        services.AddSingleton<IFileSystem, FileSystem>();
        services.RegisterAllTypes<BaseProblem>(Assembly.GetEntryAssembly()!);
        services.AddSingleton<Solver>();
        return services;
    }

    public static ServiceProvider BuildServiceProvider(IServiceCollection services) => services.BuildServiceProvider();

    public static async Task RunSolver(string[] args, IServiceCollection services)
    {
        DotEnv.Fluent()
            .WithoutExceptions()
            .WithEnvFiles()
            .WithTrimValues()
            .WithDefaultEncoding()
            .WithOverwriteExistingVars()
            .WithProbeForEnv(6)
            .Load();

        ConfigureServices(services);
        ServiceProvider serviceProvider = BuildServiceProvider(services);
        var solver = serviceProvider.GetRequiredService<Solver>();

        var all = false;
        switch (args.Length)
        {
            // dotnet run
            case 0:
                await solver.SolveLast().ConfigureAwait(false);
                break;
            // dotnet run all | dotnet run --all
            case 1 when args[0].Contains("all", StringComparison.CurrentCultureIgnoreCase):
                all = true;
                await Task.WhenAll(solver.SolveAll()).ConfigureAwait(false);
                break;
            // dotnet run 1 2 5 10
            default:
                {
                    var indexes = args.Select(arg => uint.TryParse(arg, out var index) ? index : uint.MaxValue);
                    await Task.WhenAll(solver.Solve(indexes.Where(i => i < uint.MaxValue))).ConfigureAwait(false);
                    break;
                }
        }

        solver.Render(false);
        if (all) solver.RenderOverallResults();
    }
}
