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

        services.AddSingleton<IManagerSheetEditor<AcademicSubjectData>>(new AcademicSubjectsSheet(sheetConnectData));
        services.AddSingleton<IManagerSheetEditor<GroupData>>(new GroupsSheet(sheetConnectData));
        services.AddSingleton<IManagerSheetEditor<StatementSheetData>>(statementsSheet);
        services.AddSingleton<IManagerSheetEditor<StudentData>>(studentsSheet);
        services.AddSingleton<IManagerSheetEditor<SubgroupOfPracticeData>>(new SubgroupsOfPracticeSheet(sheetConnectData));
        services.AddSingleton<IManagerSheetEditor<TeacherData>>(new TeachersSheet(sheetConnectData));

        services.AddSingleton<IGradeSheetEditor>(
            new StudentsStatementInSubgroups(statementsSheet, studentsSheet, configuration));
    }
}