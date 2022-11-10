using LazyCache;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers.Implementations;

public class StudentGroupsTableWrapper : BaseTableWrapper<GroupData>
{
    protected override TimeSpan CacheLifeTime => TimeSpan.FromMinutes(10);

    public StudentGroupsTableWrapper(IManagerSheetEditor<GroupData> sheet, IAppCache appCache, ILogger logger)
        : base(sheet, appCache, logger)
    {
    }
}