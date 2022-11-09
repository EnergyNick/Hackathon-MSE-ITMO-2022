using FluentResults;
using LazyCache;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers.Implementations;

public class SubjectsTableWrapper : BaseTableWrapper<AcademicSubjectData>
{
    private readonly string _cacheDictByGroupIdKey;

    public SubjectsTableWrapper(IManagerSheetEditor<AcademicSubjectData> sheet, IAppCache appCache, ILogger logger)
        : base(sheet, appCache, logger)
    {
        _cacheDictByGroupIdKey = $"{GetType().Name}-ByGroupId";
    }

    protected override async Task<List<AcademicSubjectData>> UpdateCache()
    {
        var result = await base.UpdateCache();
        AppCache.Add(_cacheDictByGroupIdKey, result.GroupBy(x => x.IdGroup).ToDictionary(x => x.Key, x => x.ToArray()),
            GetCacheOptions());
        return result;
    }

    public virtual async Task<Result<AcademicSubjectData[]>> ReadByGroupId(string groupId)
    {
        if (!AppCache.TryGetValue<Dictionary<string, AcademicSubjectData[]>>(_cacheDictByGroupIdKey, out var dict))
        {
            await UpdateCache();
            dict = AppCache.Get<Dictionary<string, AcademicSubjectData[]>>(_cacheDictByGroupIdKey);
        }

        return dict.TryGetValue(groupId, out var value)
            ? Result.Ok(value)
            : Result.Fail(CantFindTgErrorMessage(groupId));
    }

    private string CantFindTgErrorMessage(string id) => $"Can't find subjects in {GetType().Name} by group id {id}";
}