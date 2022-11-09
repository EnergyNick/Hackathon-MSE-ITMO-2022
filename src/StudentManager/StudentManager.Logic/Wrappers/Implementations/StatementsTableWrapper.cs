using LazyCache;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers.Implementations;

public class StatementsTableWrapper : BaseTableWrapper<StatementSheetData>
{
    public StatementsTableWrapper(IManagerSheetEditor<StatementSheetData> sheet, IAppCache appCache, ILogger logger)
        : base(sheet, appCache, logger)
    {
    }
}