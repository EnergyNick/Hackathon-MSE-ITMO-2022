using Microsoft.Extensions.Configuration;

namespace StudentManager.Tables.Models;

public record GratePartData(string Name, string Value, string MaxValue);
public record SubjectGrateData(string Name, string CurrentValue, string MaxValue, List<GratePartData> Parts);

public record SubgroupOfStudentData
{
    public string SubjectId { get; set; }
    public string? SubgroupId { get; set; }
    public SubjectGrateData SubjectGrate { get; set; }
}

public record StudentGratesData(StudentData Student, List<SubgroupOfStudentData> Subgroups);

public interface IGradeSheetEditor
{
    Task<List<StudentGratesData>> ReadAll();
};

internal class StudentsStatementInSubgroups : IGradeSheetEditor
{
    private readonly StatementsSheet _statementsSheet;
    private readonly StudentsSheet _studentsSheet;
    private readonly SheetConnectData _sheetConnect;
    
    public StudentsStatementInSubgroups(StatementsSheet statementsSheet, StudentsSheet studentsSheet,
        SheetConnectData sheetConnect)
    {
        _statementsSheet = statementsSheet;
        _studentsSheet = studentsSheet;
        _sheetConnect = sheetConnect;
    }

    public async Task<List<StudentGratesData>> ReadAll()
    {
        string GetFCsStudent(StudentData student) =>
            $"{student.Surname} {student.Name}";

        string GetPathInSheet(string idLeafSheet, string studentsStartCell) =>
            $"{idLeafSheet}!{studentsStartCell}:{studentsStartCell.Substring(studentsStartCell.ToList().FindIndex((c) => c >= '0' && c <= '9'))}";

        int GetIndexSplitPathInSheet(string startCell) =>
            startCell.ToList().FindIndex((c) => c >= '0' && c <= '9');
            
        
        string GetMaxValue(StatementSheetData statementData) => statementData.GrateType switch
        {
            GrateType.Ratio => 1.ToString(),
            GrateType.Percent => 100.ToString(),
            GrateType.Sum => statementData.MaximumGrateCell,
        };
        
        var studentsGrates = (await _studentsSheet.ReadAll()).ToDictionary(GetFCsStudent,
            (student) => new StudentGratesData(student, new()));
        var allStatements = await _statementsSheet.ReadAll();
        var statementsDetailsSheet = new DetailsStatementsSheet(_sheetConnect);
        var allStatementsDetails = (await statementsDetailsSheet.ReadAll()).
            ToDictionary((details) => details.Id);

        var studentsStatementInSubgroups = new List<SubgroupOfStudentData>();
        foreach (var statement in allStatements)
        {
            var connectStatementData = new SheetConnectData(statement.IdSheet, _sheetConnect.Configuration);
            /*var studentsStatementSheetEditor = new GoogleSheetEditor(connectStatementData,
                GetPathInSheet(statement.IdLeafSheet, statement.StudentsStartCell));
            var students = studentsStatementSheetEditor.GetSheet().Result;

            var pointsStudentsSheetEditor = new GoogleSheetEditor(connectStatementData,
                GetPathInSheet(statement.IdLeafSheet, statement.PointsStartCell));
            var pointsStudents = pointsStudentsSheetEditor.GetSheet().Result;*/

            var statementGrateSheetEditor = new GoogleSheetEditor(connectStatementData, statement.IdLeafSheet);
            var statementGrates = await statementGrateSheetEditor.GetSheet();
            int indexSplitPathInStudentsSheet = GetIndexSplitPathInSheet(statement.PointsStartCell);
            /*var students = 

            var studentGrades = new List<SubgroupOfStudentData>();
            for (int i = 0; i < students.Count && i < pointsStudents.Count; i++)
            {
                var student = students[i][0].ToString().Split();
                var pointsStudent = pointsStudents[i][0].ToString();

                if (studentsGrates.TryGetValue($"{student[0]} {student[1]}", out StudentGratesData studentData) == false)
                    break;
                
                studentData.Subgroups.Add(new SubgroupOfStudentData()
                {
                    SubjectId = statement.IdSubject,
                    SubgroupId = statement.StatementType == StatementType.Practice ? statement.IdSubgroup : null,
                    SubjectGrate = new SubjectGrateData(statement.BlockName, pointsStudent, GetMaxValue(statement), new List<GratePartData>()
                    {
                        
                    }),
                });
            }*/
        }

        return studentsGrates.Values.ToList();
    }
}