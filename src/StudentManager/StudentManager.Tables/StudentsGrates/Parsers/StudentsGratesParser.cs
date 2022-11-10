using Google.Apis.Sheets.v4;
using MajorDimensionEnum = Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum;

namespace StudentManager.Tables.Models;

internal class StudentsStatementInSubgroups : IGradeSheetEditor
{
    private record ColumnPartStatement(string Name, IList<object> Values, string MaxValue);
    
    private readonly SheetConnectData _sheetConnect;
    
    public StudentsStatementInSubgroups(SheetConnectData sheetConnect)
    {
        _sheetConnect = sheetConnect;
    }

    public async Task<List<StudentGratesData>> ReadAll(List<StudentData> allStudents,
        List<StatementSheetData> allStatements)
    {
        string GetFCsStudent(StudentData student) =>
            $"{student.Surname} {student.Name}";

        string GetPathInSheet(string studentsStartCell) =>
            $"{studentsStartCell}:{studentsStartCell.Remove(studentsStartCell.ToList().FindIndex((c) => c >= '0' && c <= '9'))}";

        string GetNameAndRangeNext(string nameSheet, string startCell) =>
            $"'{nameSheet}'!{GetPathInSheet(startCell)}";
        
        string GetNameAndRange(string nameSheet, string startCell) =>
            $"'{nameSheet}'!{startCell}";

        string GetMaxValue(StatementSheetData statementData) => statementData.GrateType switch
        {
            GrateType.Ratio => 1.ToString(),
            GrateType.Percent => 100.ToString(),
            GrateType.Sum => statementData.MaximumGrateCell,
            GrateType.FiveRatio => 5.ToString(),
        };
        
        var studentsGrates = allStudents.ToDictionary(GetFCsStudent,
            (student) => new StudentGratesData(student, new()));
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
            var service = GoogleSheetEditor.GetSheetsService(connectStatementData);
            var nameSheet = await GoogleSheetEditor.GetSheetNameByID(service, statement.SpreadsheetId, statement.SheetId);

            var studentsStatementSheetEditor = new GoogleSheetEditor(connectStatementData, service,
                GetNameAndRangeNext(nameSheet, statement.StudentsStartCell));
            var students = studentsStatementSheetEditor.GetSheet(MajorDimensionEnum.COLUMNS).Result[0];

            var pointsStudentsSheetEditor = new GoogleSheetEditor(connectStatementData, service,
                GetNameAndRangeNext(nameSheet, statement.PointsStartCell));
            var pointsStudents = pointsStudentsSheetEditor.GetSheet(MajorDimensionEnum.COLUMNS).Result[0];

            var columnsPartsStatements = new List<ColumnPartStatement>();
            if (allStatementsDetails.TryGetValue(statement.Id, out var detailsStatements))
            {
                foreach (var detailsStatement in detailsStatements)
                {
                    var details = (await new GoogleSheetEditor(connectStatementData, service,
                        GetNameAndRangeNext(nameSheet, detailsStatement.PointsStartCell)).
                        GetSheet(MajorDimensionEnum.COLUMNS))[0];

                    string maxPoints = (await new GoogleSheetEditor(connectStatementData, service,
                        GetNameAndRange(nameSheet, detailsStatement.MaximumGrateCell)).GetSheet())[0][0].ToString();
                
                    columnsPartsStatements.Add(new ColumnPartStatement(detailsStatement.Title, details, maxPoints));
                }
            }

            for (int i = 0; i < students.Count && i < pointsStudents.Count; i++)
            {
                if (students.Count < 1 || pointsStudents.Count < 1)
                    continue;;
                
                var student = students[i].ToString().Split();
                var pointsStudent = pointsStudents[i].ToString();

                if (student.Length < 2)
                    continue;

                if (studentsGrates.TryGetValue($"{student[0]} {student[1]}", out StudentGratesData studentData) == false)
                    continue;

                var studentsParts = new List<GratePartData>();
                foreach (var detailsStatement in columnsPartsStatements)
                    studentsParts.Add(new GratePartData(detailsStatement.Name,
                        detailsStatement.Values[i].ToString(), detailsStatement.MaxValue));

                studentData.Subgroups.Add(new SubgroupOfStudentData(
                    statement.IdSubject,
                    statement.StatementType == StatementType.Practice ? statement.IdSubgroup : null,
                    new SubjectGrateData(statement.BlockName, pointsStudent, GetMaxValue(statement), studentsParts)
                ));
            }
        }

        return studentsGrates.Values.ToList();
    }

    public async Task Write(string spreadsheetLink, List<StudentData> allStudents,
        List<StatementSheetData> allStatements)
    {
        var studentsGrates = await ReadAll(allStudents, allStatements);
        
        var connectStatementData = new SheetConnectData(GoogleSheetEditor.GetSpreadsheetIdFromLink(spreadsheetLink),
            _sheetConnect.Configuration);
        var sheetResult = new GoogleSheetEditor(connectStatementData, "");

        IList<IList<object>> values = new List<IList<object>>();
        
    }
}