using LazyCache;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers.Implementations;

public class TeachersTableWrapper : BaseTableWrapper<TeacherData>
{
    public TeachersTableWrapper(IManagerSheetEditor<TeacherData> sheet, IAppCache appCache, ILogger logger)
        : base(sheet, appCache, logger)
    {
    }
}