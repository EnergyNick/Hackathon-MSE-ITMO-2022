using LazyCache;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers.Implementations;

public class PracticeSubgroupsTableWrapper : BaseTableWrapper<SubgroupOfPracticeData>
{
    public PracticeSubgroupsTableWrapper(IManagerSheetEditor<SubgroupOfPracticeData> sheet, IAppCache appCache, ILogger logger)
        : base(sheet, appCache, logger)
    {
    }
}