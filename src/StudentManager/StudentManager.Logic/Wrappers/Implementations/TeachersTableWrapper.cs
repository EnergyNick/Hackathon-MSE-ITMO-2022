using FluentResults;
using LazyCache;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers.Implementations;

public class TeachersTableWrapper : BaseTableWrapper<TeacherData>
{
    private readonly string _cacheDictByTelegramIdKey;

    public TeachersTableWrapper(IManagerSheetEditor<TeacherData> sheet, IAppCache appCache, ILogger logger)
        : base(sheet, appCache, logger)
    {
        _cacheDictByTelegramIdKey = $"{GetType().Name}-ByTelegramId";
    }

    protected override async Task<List<TeacherData>> UpdateCache()
    {
        var result = await base.UpdateCache();
        AppCache.Add(_cacheDictByTelegramIdKey, result
            .Where(x => !string.IsNullOrWhiteSpace(x.Telegram))
            .DistinctBy(x => x.Telegram)
            .ToDictionary(x => x.Telegram), GetCacheOptions());
        return result;
    }

    public virtual async Task<Result<TeacherData>> ReadByTelegramId(string telegramId)
    {
        if (!AppCache.TryGetValue<Dictionary<string, TeacherData>>(_cacheDictByTelegramIdKey, out var dict))
        {
            await UpdateCache();
            dict = AppCache.Get<Dictionary<string, TeacherData>>(_cacheDictByTelegramIdKey);
        }

        if (dict == null)
            return Result.Fail<TeacherData>(WrapperErrors.EmptyInGoogleTablesCache);

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