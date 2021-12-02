namespace AdventOfCode.Lib;

public static class BaseProgram
{
    public static IServiceCollection ConfigureServices(IServiceCollection services)
    {
        services.AddAdventOfCodeHttpClient();
        services.AddSingleton<IEnvironment, Spectre.IO.Environment>();
        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton<IGlobber, Globber>();
        services.RegisterAllTypes<BaseProblem>(Assembly.GetEntryAssembly()!);
        services.AddSingleton(provider =>
        {
            var globber = provider.GetRequiredService<IGlobber>();
            var environment = provider.GetRequiredService<IEnvironment>();
            // Set working directory before constructing base problems
            SetWorkingDirectory(globber, environment);
            var problems = provider.GetRequiredService<IEnumerable<BaseProblem>>();
            return new Solver(problems);
        });
        return services;
    }

    public static ServiceProvider BuildServiceProvider(IServiceCollection services) => services.BuildServiceProvider();

    /// <summary>
    /// Set working directory to project folder so inputs file are located within the year folder
    /// </summary>
    public static void SetWorkingDirectory(IGlobber globber, IEnvironment environment)
    {
        var currentDirectory = new DirectoryPath(AppContext.BaseDirectory);
        int count = 3;
        string? str = SearchPaths();
        var workingDirectory = !string.IsNullOrEmpty(str) ? new(str) : currentDirectory;
        environment.SetWorkingDirectory(workingDirectory);

        string? SearchPaths()
        {
            var globberSettings = new GlobberSettings();
            for (; currentDirectory != null && count > 0; currentDirectory = currentDirectory.GetParent())
            {
                globberSettings.Root = currentDirectory;
                var filePath = globber.Match("*.csproj", globberSettings).OfType<FilePath>().FirstOrDefault();
                if (filePath != null)
                {
                    return filePath.FullPath;
                }
                count--;
            }
            return null;
        }
    }

    public static Task RunSolver(string[] args)
    {
        return RunSolver(args, new ServiceCollection());
    }

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
    }
}
