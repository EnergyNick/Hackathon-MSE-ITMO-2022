using StudentManager.Tables.Models;

namespace StudentManager.Tables;

public interface IGradeSheetEditor
{
    Task<List<StudentGratesData>> ReadAll();
}