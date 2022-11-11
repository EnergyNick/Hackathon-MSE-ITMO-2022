using Serilog;
using StudentManager.Core.Hosting;

namespace StudentManager.Service.Extensions;

public static class InitializationHostExtensions
{
    public static async Task RunServicesPreInitialization(this IHost host)
    {
        var logger = Log.ForContext<IPreInitializationService>();
        var initialServices = host.Services.GetServices<IPreInitializationService>().ToArray();
        logger.Information("Starting async pre initialization for {ServicesCount} services", initialServices.Length);

        try
        {
            var initializationTasks = initialServices
                .Select(x =>
                {
                    logger.Debug("Starting async pre initialization for {InitializerType}", x.GetType());
                    return x.InitializeAsync();
                })
                .ToArray();

            await Task.WhenAll(initializationTasks);

            logger.Information("Async pre initialization completed");
        }
        catch(Exception ex)
        {
            logger.Error(ex, "Async pre initialization failed");
            throw;
        }
    }

    public static IServiceCollection AddPreInitializationFor<TService>(this IServiceCollection collection,
        ServiceLifetime lifetime = ServiceLifetime.Singleton) where TService : notnull
    {
        var descriptor = new ServiceDescriptor(typeof(IPreInitializationService),
            x => x.GetRequiredService<TService>(),
            lifetime);
        collection.Add(descriptor);
        return collection;
    }
}