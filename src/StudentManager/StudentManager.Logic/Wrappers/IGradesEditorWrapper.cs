using FluentResults;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.Wrappers;

public interface IGradesEditorWrapper
{
    Task<List<StudentGratesData>> ReadAll();
    Task<Result<StudentGratesData>> ReadByUserId(string userId);
    Task<Result> WriteToSpreadsheet(string spreadsheetLink, List<StudentGratesData> studentsGrates);
};