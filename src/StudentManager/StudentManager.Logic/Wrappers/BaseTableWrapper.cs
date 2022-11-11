using FluentResults;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers;

public abstract class BaseTableWrapper<T> : ITableWrapper<T>
    where T: ISheetRowData
{
    protected readonly IManagerSheetEditor<T> Sheet;
    protected readonly IAppCache AppCache;
    protected readonly ILogger Logger;

    private readonly object _updatingLock = new();

    protected BaseTableWrapper(IManagerSheetEditor<T> sheet, IAppCache appCache, ILogger logger)
    {
        Sheet = sheet;
        AppCache = appCache;
        Logger = logger;

        CacheKey = $"{GetType().Name}-Default";
        CacheDictByIdKey = $"{GetType().Name}-ById";
    }

    public virtual async Task<Result<T>> ReadById(string id)
    {
        if (!AppCache.TryGetValue<Dictionary<string, T>>(CacheDictByIdKey, out var dict))
        {
            await UpdateCache();
            dict = AppCache.Get<Dictionary<string, T>>(CacheDictByIdKey);
        }

        if (dict == null)
            return Result.Fail<T>(WrapperErrors.EmptyInGoogleTablesCache);

        return dict.TryGetValue(id, out var value)
            ? Result.Ok(value)
            : Result.Fail(CantFindErrorMessage(id));
    }

    public virtual async Task<List<T>> ReadAll()
    {
        return AppCache.TryGetValue<List<T>>(CacheKey, out var value) ? value : await UpdateCache();
    }

    public virtual async Task<Result> Update(string id, T value)
    {
        var previousValue = await ReadById(id);

        if (previousValue.IsFailed)
            return previousValue.ToResult();

        try
        {
            await Sheet.Update(value, id);
            //Check if this necessary after all updates
            await UpdateCache();
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error on {ServiceName} while updating value by id {Id}", GetType().Name, id);
        }

        return Result.Ok();
    }

    protected virtual async Task<List<T>> UpdateCache()
    {
        List<T> items;
        try
        {
            items = await Sheet.ReadAll();
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error on getting all items in {ServiceName}", GetType().Name);
            return new List<T>();
        }

        Logger.Information("Cache of {Service} updated, rate of refresh is {Time}", GetType().Name, CacheLifeTime);

        AppCache.Add(CacheKey, items, GetCacheOptions());
        AppCache.Add(CacheDictByIdKey, items.ToDictionary(x => x.Id), GetCacheOptions());
        return items;
    }

    protected virtual string CacheKey { get; }
    protected virtual string CacheDictByIdKey { get; }
    protected virtual TimeSpan CacheLifeTime { get; } = TimeSpan.FromMinutes(5);

    protected virtual MemoryCacheEntryOptions GetCacheOptions() =>
        new LazyCacheEntryOptions()
            .SetAbsoluteExpiration(CacheLifeTime, ExpirationMode.ImmediateEviction)
            .RegisterPostEvictionCallback((key, _, reason, _) =>
            {
                if (reason is not (EvictionReason.Expired or EvictionReason.TokenExpired)) return;
                lock (_updatingLock)
                    AppCache.GetOrAddAsync(key as string, async _ => await ReadAll(), GetCacheOptions());
            });

    protected virtual string CantFindErrorMessage(string id)
    {
        Logger.Warning("Can\'t find element in {Name} by id {Id}", GetType().Name, id);
        return WrapperErrors.CantFindItemById;
    }
}