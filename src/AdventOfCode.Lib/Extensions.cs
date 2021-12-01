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
            c.BaseAddress = new Uri(Constants.Endpoint);
            c.DefaultRequestHeaders.Add("Cookie", $"session={Constants.SessionCookie}");
        });
}
