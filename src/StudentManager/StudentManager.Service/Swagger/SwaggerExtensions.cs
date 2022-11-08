using Microsoft.OpenApi.Models;
using StudentManager.Service.Extensions;

namespace StudentManager.Service.Swagger;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerSupport(this IServiceCollection services, IWebHostEnvironment environment)
    {
        if(environment.IsDevelopment() || environment.IsTesting())
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StudentManager.Service", Version = "v1" });
                c.SchemaFilter<SwaggerSchemaFilter>();
                c.EnableAnnotations(true, true);
                c.UseInlineDefinitionsForEnums();
            });
        }
        return services;
    }

    public static IApplicationBuilder AddSwaggerSupport(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if(environment.IsDevelopment() || environment.IsTesting())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentManager.Service v1"));
        }
        return app;
    }
}