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
    private record ColumnPartStatemenet(string Name, List<string> Values, string MaxValue);
    
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

        string GetPathInSheet(string studentsStartCell) =>
            $"{studentsStartCell}:{studentsStartCell.Remove(studentsStartCell.ToList().FindIndex((c) => c >= '0' && c <= '9'))}";

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
        var allStatementsDetailsList = await statementsDetailsSheet.ReadAll();
        var allStatementsDetails = new Dictionary<string, List<DetailsStatementData>>();
        foreach (var detailsStatementData in allStatementsDetailsList)
        {
            if (allStatementsDetails.TryGetValue(detailsStatementData.IdStatement, out var list) == false)
            {
                list = new List<DetailsStatementData>();
                allStatementsDetails.Add(detailsStatementData.IdStatement, list);
            }
            list.Add(detailsStatementData);
        }

        foreach (var statement in allStatements)
        {
            var connectStatementData = new SheetConnectData(statement.SpreadsheetId, _sheetConnect.Configuration);
            var studentsStatementSheetEditor = new GoogleSheetEditor(connectStatementData, statement.SheetId,
                GetPathInSheet(statement.StudentsStartCell));
            var students = studentsStatementSheetEditor.GetSheet().Result;

            var pointsStudentsSheetEditor = new GoogleSheetEditor(connectStatementData, statement.SheetId,
                GetPathInSheet(statement.PointsStartCell));
            var pointsStudents = pointsStudentsSheetEditor.GetSheet().Result;

            var columnsPartsStatements = new List<ColumnPartStatemenet>();
            foreach (var detailsStatement in allStatementsDetails[statement.Id])
            {
                var details = await new GoogleSheetEditor(connectStatementData, statement.SheetId,
                    GetPathInSheet(detailsStatement.PointsStartCell)).GetSheet();
                var list = new List<string>();
                foreach (var detail in details)
                    if (detail.Count > 0)
                        list.Add(detail[0].ToString());
                    else
                        list.Add("");

                string maxPoints = (await new GoogleSheetEditor(connectStatementData, statement.SheetId,
                    GetPathInSheet(detailsStatement.MaximumGrateCell)).GetSheet())[0][0].ToString();
                
                columnsPartsStatements.Add(new ColumnPartStatemenet(detailsStatement.Title, list, maxPoints));
            }

            for (int i = 0; i < students.Count && i < pointsStudents.Count; i++)
            {
                var student = students[i][0].ToString().Split();
                var pointsStudent = pointsStudents[i][0].ToString();

                if (student.Length < 2)
                    continue;

                if (studentsGrates.TryGetValue($"{student[0]} {student[1]}", out StudentGratesData studentData) == false)
                    continue;

                var studentsParts = new List<GratePartData>();
                foreach (var detailsStatement in columnsPartsStatements)
                    studentsParts.Add(new GratePartData(detailsStatement.Name,
                        detailsStatement.Values[i], detailsStatement.MaxValue));

                studentData.Subgroups.Add(new SubgroupOfStudentData()
                {
                    SubjectId = statement.IdSubject,
                    SubgroupId = statement.StatementType == StatementType.Practice ? statement.IdSubgroup : null,
                    SubjectGrate = new SubjectGrateData(statement.BlockName, pointsStudent, GetMaxValue(statement), studentsParts),
                });
            }
        }

        return studentsGrates.Values.ToList();
    }
}