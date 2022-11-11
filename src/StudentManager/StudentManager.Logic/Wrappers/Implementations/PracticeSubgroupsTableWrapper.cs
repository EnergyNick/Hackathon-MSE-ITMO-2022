using FluentResults;
using LazyCache;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers.Implementations;

public class PracticeSubgroupsTableWrapper : BaseTableWrapper<SubgroupOfPracticeData>
{
    private readonly string _cacheDictByTeacherIdKey;

    public PracticeSubgroupsTableWrapper(IManagerSheetEditor<SubgroupOfPracticeData> sheet, IAppCache appCache, ILogger logger)
        : base(sheet, appCache, logger)
    {
        _cacheDictByTeacherIdKey = $"{GetType().Name}-ByTeacherId";
    }

    protected override async Task<List<SubgroupOfPracticeData>> UpdateCache()
    {
        var result = await base.UpdateCache();
        AppCache.Add(_cacheDictByTeacherIdKey,
            result.GroupBy(x => x.IdTeacher).ToDictionary(x => x.Key, x => x.ToArray()),
            GetCacheOptions());
        return result;
    }

    public virtual async Task<Result<SubgroupOfPracticeData[]>> ReadByTeacherId(string teacherId)
    {
        if (!AppCache.TryGetValue<Dictionary<string, SubgroupOfPracticeData[]>>(_cacheDictByTeacherIdKey, out var dict))
        {
            await UpdateCache();
            dict = AppCache.Get<Dictionary<string, SubgroupOfPracticeData[]>>(_cacheDictByTeacherIdKey);
        }

        if (dict == null)
            return Result.Fail<SubgroupOfPracticeData[]>(WrapperErrors.EmptyInGoogleTablesCache);

        return dict.TryGetValue(teacherId, out var value)
            ? value
            : Array.Empty<SubgroupOfPracticeData>();
    }
}