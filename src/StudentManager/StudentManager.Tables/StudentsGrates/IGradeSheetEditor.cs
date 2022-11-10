using StudentManager.Tables.Models;

namespace StudentManager.Tables;

public interface IGradeSheetEditor
{
    Task<List<StudentGratesData>> ReadAll(List<StudentData> allStudents,
        List<StatementSheetData> allStatements);
    Task Write(string spreadsheetLink, List<StudentData> allStudents,
        List<StatementSheetData> allStatements);
}