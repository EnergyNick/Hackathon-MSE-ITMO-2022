namespace StudentManager.Service.Extensions;

public static class HostSettingsExtensions
{
    private static readonly string[] Origins =
    {
        "https://*.projects-cabinet.ru"
    };

    private static readonly string[] DevOrigins =
    {
        "http://localhost:3000"
    };

    public static IServiceCollection AddCorsValidations(this IServiceCollection services,
        IWebHostEnvironment environment) =>
        services.AddCors(opt => opt.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins(environment.IsDevelopment() || environment.IsTesting()
                    ? Origins.Concat(DevOrigins).ToArray()
                    : Origins)
                .SetIsOriginAllowedToAllowWildcardSubdomains();
        }));
}