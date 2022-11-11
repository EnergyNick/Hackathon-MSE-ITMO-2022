using StudentManager.Tables.Models;

namespace StudentManager.Tables;

public interface IGradeSheetEditor
{
    Task<List<StudentGratesData>> ReadAll(List<StudentData> allStudents,
        List<StatementSheetData> allStatements);
    Task WriteToSpreadsheet(string spreadsheetLink, List<StudentGratesData> studentsGrates,
        List<StatementSheetData> allStatements, List<SubgroupOfPracticeData> allSubgroups,
        List<TeacherData> allTeachers);
}