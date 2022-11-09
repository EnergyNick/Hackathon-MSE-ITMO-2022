using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManager.Tables.Models;

namespace StudentManager.Tables;

public static class ServiceCollectionExtensions
{
    private const string _spreadsheetId = "1Z4tV3gmqqDrfTH8W-clmsb6SK-e1r0MCcxEI4kiFVVw";

    public static void AddGoogleSheetsEditors(this IServiceCollection services, IConfiguration configuration)
    {
        var connectData = new SheetConnectData(_spreadsheetId, configuration);

        var statementsSheet = new StatementsSheet(connectData);
        var studentsSheet = new StudentsSheet(connectData);

        services.AddSingleton<IManagerSheetEditor<AcademicSubjectData>>(new AcademicSubjectsSheet(connectData));
        services.AddSingleton<IManagerSheetEditor<GroupData>>(new GroupsSheet(connectData));
        services.AddSingleton<IManagerSheetEditor<StatementSheetData>>(statementsSheet);
        services.AddSingleton<IManagerSheetEditor<StudentData>>(studentsSheet);
        services.AddSingleton<IManagerSheetEditor<SubgroupOfPracticeData>>(new SubgroupsOfPracticeSheet(connectData));
        services.AddSingleton<IManagerSheetEditor<TeacherData>>(new TeachersSheet(connectData));

        services.AddSingleton<IGradeSheetEditor>(
            new StudentsStatementInSubgroups(statementsSheet, studentsSheet, connectData));
    }
}