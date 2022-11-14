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
        services.AddPreInitializationFor<StudentsTableWrapper>();
        services.AddSingleton<TeachersTableWrapper>();
        services.AddPreInitializationFor<TeachersTableWrapper>();
        services.AddSingleton<SubjectsTableWrapper>();
        services.AddPreInitializationFor<SubjectsTableWrapper>();
        services.AddSingleton<StatementsTableWrapper>();
        services.AddPreInitializationFor<StatementsTableWrapper>();
        services.AddSingleton<PracticeSubgroupsTableWrapper>();
        services.AddPreInitializationFor<PracticeSubgroupsTableWrapper>();
        services.AddSingleton<StudentGroupsTableWrapper>();
        services.AddPreInitializationFor<StudentGroupsTableWrapper>();

        services.AddSingleton<IGradesEditorWrapper, GradesEditorWrapper>();
        services.AddPreInitializationFor<IGradesEditorWrapper>();
    }
}