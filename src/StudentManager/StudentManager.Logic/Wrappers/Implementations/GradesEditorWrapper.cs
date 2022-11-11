using FluentResults;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers.Implementations;

public class GradesEditorWrapper : IGradesEditorWrapper
{
    protected readonly IGradeSheetEditor Sheet;
    protected readonly IAppCache AppCache;
    protected readonly ILogger Logger;

    private readonly ITableWrapper<StatementSheetData> _statements;
    private readonly ITableWrapper<StudentData> _students;
    private readonly ITableWrapper<TeacherData> _teachers;
    private readonly ITableWrapper<SubgroupOfPracticeData> _subgroups;

    public GradesEditorWrapper(IGradeSheetEditor sheet,
        StudentsTableWrapper students,
        StatementsTableWrapper statements,
        TeachersTableWrapper teachers,
        PracticeSubgroupsTableWrapper subgroups,
        IAppCache appCache, ILogger logger)
    {
        Sheet = sheet;
        _students = students;
        _statements = statements;
        _teachers = teachers;
        _subgroups = subgroups;
        AppCache = appCache;
        Logger = logger;

        CacheKey = $"{GetType().Name}-Default";
        CacheDictByUserIdKey = $"{GetType().Name}-ByUserNameDefault";
    }

    public virtual async Task<List<StudentGratesData>> ReadAll()
    {
        return AppCache.TryGetValue<List<StudentGratesData>>(CacheKey, out var value) ? value : await UpdateCache();
    }

    public async Task<Result<StudentGratesData>> ReadByUserId(string userId)
    {
        if (!AppCache.TryGetValue<Dictionary<string,StudentGratesData>>(CacheDictByUserIdKey, out var dict))
        {
            await UpdateCache();
            dict = AppCache.Get<Dictionary<string,StudentGratesData>>(CacheDictByUserIdKey);
        }

        if (dict == null)
            return Result.Fail<StudentGratesData>(WrapperErrors.EmptyInGoogleTablesCache);

        return dict.TryGetValue(userId, out var value)
            ? Result.Ok(value)
            : Result.Fail(CantFindByUserErrorMessage(userId));
    }

    public async Task<Result> WriteToSpreadsheet(string spreadsheetLink, List<StudentGratesData> studentsGrates)
    {
        try
        {
            await Sheet.WriteToSpreadsheet(spreadsheetLink, studentsGrates,
                await _statements.ReadAll(),
                await _subgroups.ReadAll(),
                await _teachers.ReadAll());
            return Result.Ok();
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error on trying to create unload for {Link}", spreadsheetLink);
            return Result.Fail(WrapperErrors.ErrorOnUploadingGradesToTable);
        }
    }

    protected virtual async Task<List<StudentGratesData>> UpdateCache()
    {
        List<StudentGratesData> items;
        try
        {
            var students = await _students.ReadAll();
            var statements = await _statements.ReadAll();
            items = await Sheet.ReadAll(students, statements);
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error on getting all items in {ServiceName}", GetType().Name);
            return new List<StudentGratesData>();
        }

        AppCache.Add(CacheKey, items, GetCacheOptions());
        AppCache.Add(CacheDictByUserIdKey, items.ToDictionary(x => x.Student.Id),
            GetCacheOptions());
        return items;
    }

    protected virtual string CacheKey { get; }
    protected virtual string CacheDictByUserIdKey { get; }
    protected virtual TimeSpan CacheLifeTime { get; } = TimeSpan.FromMinutes(4);

    protected virtual MemoryCacheEntryOptions GetCacheOptions() =>
        new LazyCacheEntryOptions()
            .SetAbsoluteExpiration(CacheLifeTime, ExpirationMode.ImmediateEviction)
            .RegisterPostEvictionCallback((key, _, reason, _) =>
            {
                if (reason is EvictionReason.Expired or EvictionReason.TokenExpired)
                {
                    Logger.Information("Cache of {Service} updated after {Time}", GetType().Name, CacheLifeTime);
                    AppCache.GetOrAddAsync(key as string, async _ => await ReadAll(), GetCacheOptions());
                }
            });

    protected virtual string CantFindByUserErrorMessage(string id)
    {
        Logger.Warning("Can\'t find element in {Name} by id {Id}", GetType().Name, id);
        return WrapperErrors.CantFindGradesByUserId;
    }
}