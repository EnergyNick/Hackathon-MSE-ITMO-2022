using LazyCache;
using Serilog;
using StudentManager.Tables;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.TableWrappers.Implementations;

public class SubjectsTableWrapper : BaseTableWrapper<AcademicSubjectData>
{
    public SubjectsTableWrapper(IGoogleSheet<AcademicSubjectData> sheet, IAppCache appCache, ILogger logger)
        : base(sheet, appCache, logger)
    {
    }
}