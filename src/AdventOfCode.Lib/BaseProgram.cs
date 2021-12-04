namespace AdventOfCode.Lib;

public static class BaseProgram
{
    public static IServiceCollection ConfigureServices(IServiceCollection services)
    {
        services.AddAdventOfCodeHttpClient();
        services.RegisterAllTypes<BaseProblem>(Assembly.GetEntryAssembly()!);
        services.AddSingleton<Solver>();
        return services;
    }

    public static ServiceProvider BuildServiceProvider(IServiceCollection services) => services.BuildServiceProvider();

    /// <summary>
    /// Set working directory to project folder so inputs file are located within the year folder
    /// </summary>
    private static void SetWorkingDirectory()
    {
        var workingDirectory = Extensions.FindDirectory("*.csproj", 4);
        System.IO.Directory.SetCurrentDirectory(workingDirectory);
    }

    public static async Task<int> RunSolver(string[] args)
    {
        return await RunSolver(args, new ServiceCollection());
    }

    public static async Task<int> RunSolver(string[] args, IServiceCollection services)
    {
        DotEnv.Fluent()
            .WithoutExceptions()
            .WithEnvFiles()
            .WithTrimValues()
            .WithDefaultEncoding()
            .WithOverwriteExistingVars()
            .WithProbeForEnv(6)
            .Load();

        SetWorkingDirectory();

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
                await solver.SolveAll().ConfigureAwait(false);
                break;
            // dotnet run 1 2 5 10
            default:
                {
                    var indexes = args.Select(arg => uint.TryParse(arg, out var index) ? index : uint.MaxValue);
                    await solver.Solve(indexes.Where(i => i < uint.MaxValue)).ConfigureAwait(false);
                    break;
                }
        }

        // solver.Render(false);
        if (all) solver.RenderOverallResults();

        return 0;
    }
}
