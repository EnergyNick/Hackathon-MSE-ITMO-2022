using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManager.Tables.Models;

namespace StudentManager.Tables;

public static class ServiceCollectionExtensions
{
    private const string _spreadsheetId = "1Z4tV3gmqqDrfTH8W-clmsb6SK-e1r0MCcxEI4kiFVVw";

    public static void AddGoogleSheetsEditors(this IServiceCollection services, IConfiguration configuration)
    {
        var sheetConnectData = new SheetConnectData(_spreadsheetId, configuration);
        
        services.AddSingleton(new AcademicSubjectSheet(sheetConnectData));
        services.AddSingleton(new GroupsSheet(sheetConnectData));
        services.AddSingleton(new StatementsSheet(sheetConnectData));
        services.AddSingleton(new StudentsSheet(sheetConnectData));
        services.AddSingleton(new SubgroupsOfPracticeSheet(sheetConnectData));
        services.AddSingleton(new TeachersSheet(sheetConnectData));
    }
}