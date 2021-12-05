namespace AdventOfCode.Lib;

public static class Extensions
{
    public static void RegisterAllTypes<T>(this IServiceCollection services, Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Type serviceType = typeof(T);
        var serviceDescriptors = assembly.GetTypes()
            .Where(type => type.IsAssignableTo(serviceType) && !type.IsInterface && !type.IsAbstract)
            .Select(implementationType => new ServiceDescriptor(serviceType, implementationType, lifetime));
        foreach (var serviceDescriptor in serviceDescriptors)
            services.Add(serviceDescriptor);
    }

    public static void AddAdventOfCodeHttpClient(this IServiceCollection services) =>
        services.AddHttpClient<IAdventClient, AdventClient>(c =>
        {
            c.BaseAddress = new(Constants.Endpoint);
            c.DefaultRequestHeaders.Add("Cookie", $"session={Constants.SessionCookie}");
        });

    public static string FindDirectory(string pattern, int depth)
    {
        var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);
        var baseDirectory = currentDirectory.FullName;
        var count = depth;
        string? str = SearchPaths();
        return !string.IsNullOrEmpty(str) ? str : baseDirectory;

        string? SearchPaths()
        {
            for (; currentDirectory != null && count > 0; currentDirectory = currentDirectory.Parent)
            {
                var files = currentDirectory.GetFiles(pattern, SearchOption.TopDirectoryOnly);
                if (files.Length > 0)
                    return currentDirectory.FullName;
                count--;
            }
            return null;
        }
    }
}
