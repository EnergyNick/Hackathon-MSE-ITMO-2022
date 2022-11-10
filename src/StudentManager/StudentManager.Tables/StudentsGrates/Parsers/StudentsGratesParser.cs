namespace StudentManager.Tables.Models;

internal class StudentsStatementInSubgroups : IGradeSheetEditor
{
    private record ColumnPartStatement(string Name, List<string> Values, string MaxValue);
    
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
            var service = GoogleSheetEditor.GetSheetsService(connectStatementData);
            var nameSheet = await GoogleSheetEditor.GetSheetNameByID(service, statement.SpreadsheetId, statement.SheetId);

            var studentsStatementSheetEditor = new GoogleSheetEditor(connectStatementData, service,
                GetNameAndRangeNext(nameSheet, statement.StudentsStartCell));
            var students = studentsStatementSheetEditor.GetSheet().Result;

            var pointsStudentsSheetEditor = new GoogleSheetEditor(connectStatementData, service,
                GetNameAndRangeNext(nameSheet, statement.PointsStartCell));
            var pointsStudents = pointsStudentsSheetEditor.GetSheet().Result;

            var columnsPartsStatements = new List<ColumnPartStatement>();
            if (allStatementsDetails.TryGetValue(statement.Id, out var detailsStatements))
            {
                foreach (var detailsStatement in detailsStatements)
                {
                    var details = await new GoogleSheetEditor(connectStatementData, service,
                        GetNameAndRangeNext(nameSheet, detailsStatement.PointsStartCell)).GetSheet();
                    var list = new List<string>();
                    foreach (var detail in details)
                        if (detail.Count > 0)
                            list.Add(detail[0].ToString());
                        else
                            list.Add("");

                    string maxPoints = (await new GoogleSheetEditor(connectStatementData, service,
                        GetNameAndRange(nameSheet, detailsStatement.MaximumGrateCell)).GetSheet())[0][0].ToString();
                
                    columnsPartsStatements.Add(new ColumnPartStatement(detailsStatement.Title, list, maxPoints));
                }
            }

            for (int i = 0; i < students.Count && i < pointsStudents.Count; i++)
            {
                if (students[i].Count < 1 || pointsStudents.Count < 1)
                    continue;;
                
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

                studentData.Subgroups.Add(new SubgroupOfStudentData(
                    statement.IdSubject,
                    statement.StatementType == StatementType.Practice ? statement.IdSubgroup : null,
                    new SubjectGrateData(statement.BlockName, pointsStudent, GetMaxValue(statement), studentsParts)
                ));
            }
        }

        return studentsGrates.Values.ToList();
    }
}