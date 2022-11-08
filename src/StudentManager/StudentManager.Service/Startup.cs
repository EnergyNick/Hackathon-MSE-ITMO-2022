using Microsoft.AspNetCore.HttpOverrides;
using StudentManager.Service.Extensions;
using StudentManager.Service.Logger;
using StudentManager.Service.Swagger;

namespace StudentManager.Service;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps("StudentManager.Service", "StudentManager.Core");
        });

        services.AddLazyCache();

        services.AddHttpClient();

        services.AddScoped<LoggingMiddleware>();
        services.AddCorsValidations(Environment);

        services.AddSwaggerSupport(Environment);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseMiddleware<LoggingMiddleware>();

        if (Environment.IsDevelopment() || Environment.IsTesting())
            app.UseDeveloperExceptionPage();

        app.AddSwaggerSupport(Environment);

        app.UseCors() ;

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}