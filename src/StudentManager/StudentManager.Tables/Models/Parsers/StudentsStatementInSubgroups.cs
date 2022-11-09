using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace StudentManager.Tables.Models;

public record GratePartData(string Name, string Value, string MaxValue);
public record SubjectGrateData(string Name, string CurrentValue, string MaxValue, List<GratePartData> Parts);

public record StudentGradeInSubgroupData
{
    public StudentData StudentData { get; set; }
    public string? SubgroupId { get; set; }
    public List<SubjectGrateData> SubjectGrates { get; set; }
}

public record SubjectStatementData(List<StudentGradeInSubgroupData> Students);

public interface IGradeSheetEditor
{
    Task<List<SubjectStatementData>> ReadAll();
};

internal class StudentsStatementInSubgroups : IGradeSheetEditor
{
    private readonly StatementsSheet _statementsSheet;
    private readonly StudentsSheet _studentsSheet;
    private readonly IConfiguration _configuration;
    
    public StudentsStatementInSubgroups(StatementsSheet statementsSheet, StudentsSheet studentsSheet,
        IConfiguration configuration)
    {
        _statementsSheet = statementsSheet;
        _studentsSheet = studentsSheet;
        _configuration = configuration;
    }

    public async Task<List<SubjectStatementData>> ReadAll()
    {
        string GetFCsStudent(StudentData student) =>
            $"{student.Surname} {student.Name}{(student.Patronymic != "" ? $" {student.Patronymic}" : "")}";

        var allStudents = await _studentsSheet.ReadAll();
        var allStudentsDictionary = allStudents.
            ToDictionary(GetFCsStudent);
        var allStatements = await _statementsSheet.ReadAll();

        var studentsStatementInSubgroups = new List<SubjectStatementData>();
        foreach (var statement in allStatements)
        {
            var connectStatementData = new SheetConnectData(statement.IdSheet, _configuration);
            var studentsStatementSheetEditor = new GoogleSheetEditor(connectStatementData,
                $"{statement.IdLeafSheet}!{statement.StudentsStartCell}:{statement.StudentsStartCell[0]}");
            var students = studentsStatementSheetEditor.GetSheet().Result;

            var pointsStudentsSheetEditor = new GoogleSheetEditor(connectStatementData,
                $"{statement.IdLeafSheet}!{statement.PointsStartCell}:{statement.PointsStartCell[0]}");
            var pointsStudents = pointsStudentsSheetEditor.GetSheet().Result;

            var studentGrades = new List<StudentGradeInSubgroupData>();
            for (int i = 0; i < students.Count && i < pointsStudents.Count; i++)
            {
                var student = students[i][0].ToString();
                var pointsStudent = pointsStudents[i][0].ToString();

                if (allStudentsDictionary.TryGetValue(student, out StudentData studentData))
                {
                    studentGrades.Add(new StudentGradeInSubgroupData()
                    {
                        StudentData = studentData,
                        SubgroupId = statement.IdSubgroup,
                        SubjectGrates = new List<SubjectGrateData>()
                        {
                            
                        },
                    });
                }
            }
        }

        return studentsStatementInSubgroups;
    }
}