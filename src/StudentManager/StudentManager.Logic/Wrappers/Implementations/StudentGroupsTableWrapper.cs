using LazyCache;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers.Implementations;

public class StudentGroupsTableWrapper : BaseTableWrapper<GroupData>
{
    public StudentGroupsTableWrapper(IManagerSheetEditor<GroupData> sheet, IAppCache appCache, ILogger logger)
        : base(sheet, appCache, logger)
    {
    }
}