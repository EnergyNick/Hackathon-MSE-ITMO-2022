using LazyCache;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.TableWrappers.Implementations;

public class TeachersTableWrapper : BaseTableWrapper<TeacherData>
{
    public TeachersTableWrapper(IGoogleSheet<TeacherData> sheet, IAppCache appCache, ILogger logger)
        : base(sheet, appCache, logger)
    {
    }
}