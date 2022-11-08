using LazyCache;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.TableWrappers.Implementations;

public class StudentsTableWrapper : BaseTableWrapper<StudentData>
{
    public StudentsTableWrapper(IGoogleSheet<StudentData> sheet, IAppCache appCache, ILogger logger)
        : base(sheet, appCache, logger)
    {
    }
}