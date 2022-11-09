using FluentResults;
using LazyCache;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers.Implementations;

public class StatementsTableWrapper : BaseTableWrapper<StatementSheetData>
{
    private readonly string _cacheDictBySubjectIdKey;
    public StatementsTableWrapper(IManagerSheetEditor<StatementSheetData> sheet, IAppCache appCache, ILogger logger)
        : base(sheet, appCache, logger)
    {
        _cacheDictBySubjectIdKey = $"{GetType().Name}-BySubjectId";
    }

    protected override async Task<List<StatementSheetData>> UpdateCache()
    {
        var result = await base.UpdateCache();
        AppCache.Add(_cacheDictBySubjectIdKey,
            result.GroupBy(x => x.IdSubject).ToDictionary(x => x.Key, x => x.ToArray()), GetCacheOptions());
        return result;
    }

    public virtual async Task<Result<StatementSheetData[]>> ReadBySubjectId(string subjectId)
    {
        if (!AppCache.TryGetValue<Dictionary<string, StatementSheetData[]>>(_cacheDictBySubjectIdKey, out var dict))
        {
            await UpdateCache();
            dict = AppCache.Get<Dictionary<string, StatementSheetData[]>>(_cacheDictBySubjectIdKey);
        }

        return dict.TryGetValue(subjectId, out var value)
            ? Result.Ok(value)
            : Result.Fail(CantFindSubjectErrorMessage(subjectId));
    }

    private string CantFindSubjectErrorMessage(string id) =>
        $"Can't find statement in {GetType().Name} by subject id {id}";
}