using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManager.Tables.Models;

namespace StudentManager.Tables;

public static class ServiceCollectionExtensions
{
    public const string SpreadsheetId = "1Z4tV3gmqqDrfTH8W-clmsb6SK-e1r0MCcxEI4kiFVVw";

    public static void AddGoogleSheetsEditors(this IServiceCollection services, IConfiguration configuration)
    {
        var sheetConnectData = new SheetConnectData(SpreadsheetId, configuration);

        var statementsSheet = new StatementsSheet(sheetConnectData);
        var studentsSheet = new StudentsSheet(sheetConnectData);
        
        services.AddSingleton(new AcademicSubjectsSheet(sheetConnectData));
        services.AddSingleton(new GroupsSheet(sheetConnectData));
        services.AddSingleton(statementsSheet);
        services.AddSingleton(studentsSheet);
        services.AddSingleton(new SubgroupsOfPracticeSheet(sheetConnectData));
        services.AddSingleton(new TeachersSheet(sheetConnectData));
        
        services.AddSingleton(new StudentsStatementInSubgroups(statementsSheet, studentsSheet, configuration));
    }
}