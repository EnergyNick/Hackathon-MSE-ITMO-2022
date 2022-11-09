using StudentManager.Logic.Wrappers;
using StudentManager.Logic.Wrappers.Implementations;

namespace StudentManager.Service.Extensions;

public static class ServiceCollectionExtensions
{
    // public static void Add(this IServiceCollection services, IConfiguration cfg)
    // {
    // }

    public static void AddTableWrappers(this IServiceCollection services)
    {
        services.AddSingleton<StudentsTableWrapper>();
        services.AddSingleton<TeachersTableWrapper>();
        services.AddSingleton<SubjectsTableWrapper>();
        services.AddSingleton<StatementsTableWrapper>();
        services.AddSingleton<PracticeSubgroupsTableWrapper>();
        services.AddSingleton<StudentGroupsTableWrapper>();

        services.AddSingleton<IGradesEditorWrapper, GradesEditorWrapper>();
    }
}