using FluentResults;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers.Implementations;

public class GradesEditorWrapper : IGradesEditorWrapper
{
    protected readonly IGradeSheetEditor Sheet;
    protected readonly IAppCache AppCache;
    protected readonly ILogger Logger;

    public GradesEditorWrapper(IGradeSheetEditor sheet, IAppCache appCache, ILogger logger)
    {
        Sheet = sheet;
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

        return dict != null && dict.TryGetValue(userId, out var value)
            ? Result.Ok(value)
            : Result.Fail(CantFindByUserErrorMessage(userId));
    }

    protected virtual async Task<List<StudentGratesData>> UpdateCache()
    {
        List<StudentGratesData> items;
        try
        {
            items = await Sheet.ReadAll();
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
    protected virtual TimeSpan CacheLifeTime { get; } = TimeSpan.FromMinutes(2);

    protected virtual MemoryCacheEntryOptions GetCacheOptions() =>
        new LazyCacheEntryOptions()
            .SetAbsoluteExpiration(CacheLifeTime, ExpirationMode.ImmediateEviction)
            .RegisterPostEvictionCallback((key, _, reason, _) =>
            {
                if (reason is EvictionReason.Expired or EvictionReason.TokenExpired)
                    AppCache.GetOrAddAsync(key as string, async _ => await ReadAll(), GetCacheOptions());
            });

    protected virtual string CantFindByUserErrorMessage(string id)
    {
        Logger.Warning("Can\'t find element in {Name} by id {Id}", GetType().Name, id);
        return $"CANT_FIND_GRADES_BY_USER_ID";
    }
}