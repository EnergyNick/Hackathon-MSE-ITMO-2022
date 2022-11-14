using FluentResults;
using LazyCache;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers.Implementations;

public class StudentsTableWrapper : BaseTableWrapper<StudentData>
{
    private readonly string _cacheDictByTelegramIdKey;

    public StudentsTableWrapper(IManagerSheetEditor<StudentData> sheet, IAppCache appCache, ILogger logger)
        : base(sheet, appCache, logger)
    {
        _cacheDictByTelegramIdKey = $"{GetType().Name}-ByTelegramId";
    }

    protected override async Task<List<StudentData>> UpdateCache()
    {
        var result = await base.UpdateCache();
        AppCache.Add(_cacheDictByTelegramIdKey, result
            .DistinctBy(x => x.Telegram)
            .ToDictionary(x => x.Telegram), GetCacheOptions());
        return result;
    }

    public virtual async Task<Result<StudentData>> ReadByTelegramId(string telegramId)
    {
        if (!AppCache.TryGetValue<Dictionary<string, StudentData>>(_cacheDictByTelegramIdKey, out var dict))
            return Result.Fail<StudentData>(WrapperErrors.EmptyInGoogleTablesCache);

        return dict.TryGetValue(telegramId, out var value)
            ? Result.Ok(value)
            : Result.Fail(CantFindTgErrorMessage(telegramId));
    }

    private string CantFindTgErrorMessage(string id)
    {
        Logger.Warning("Can\'t find telegram in {Name} by id {Id}", GetType().Name, id);
        return WrapperErrors.CantFindUserByTelegramUsername;
    }
}