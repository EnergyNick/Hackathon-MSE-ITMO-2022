using Mcrio.Configuration.Provider.Docker.Secrets;
using Serilog;
using StudentManager.Service;
using StudentManager.Service.Extensions;
using StudentManager.Service.Logger;


Log.Logger = LoggerBuilder.CreateLogger();

try
{
    var host = CreateHostBuilder(args).Build();
    await host.RunServicesPreInitialization();
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(config => config
            .AddJsonFile("Properties/PrivateSettings.json", true, true)
            .AddDockerSecrets())
        .UseSerilog(Log.Logger)
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());