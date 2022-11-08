using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManager.Tables.Models;

namespace StudentManager.Tables;

public static class ServiceCollectionExtensions
{
    public static void AddGoogleSheetsEditors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new AcademicSubjectSheet(configuration));
    }
}