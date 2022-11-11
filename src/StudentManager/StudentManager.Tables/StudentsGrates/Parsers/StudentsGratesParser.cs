using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using MajorDimensionEnum = Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum;
using BatchMajorDimensionEnum = Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.BatchGetRequest.MajorDimensionEnum;

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
        
        string ParsePointsAndNumber(string point) =>
            new string(point.Replace('.', ',').Where((c) => c is >= '0' and <= '9' or ',').ToArray());

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
            
            var studentsRangesSheetEditor = new GoogleSheetEditor(connectStatementData, service);

            List<string> ranges = new List<string>();
            ranges.Add(GetNameAndRangeNext(nameSheet, statement.StudentsStartCell));
            ranges.Add(GetNameAndRangeNext(nameSheet, statement.PointsStartCell));

            var columnsPartsStatements = new List<ColumnPartStatement>();
            if (allStatementsDetails.TryGetValue(statement.Id, out var detailsStatements))
            {
                foreach (var detailsStatement in detailsStatements)
                {
                    ranges.Add(GetNameAndRangeNext(nameSheet, detailsStatement.PointsStartCell));
                    ranges.Add(GetNameAndRange(nameSheet, detailsStatement.MaximumGrateCell));
                }
            }
            
            if (statement.GrateType == GrateType.Sum)
                ranges.Add(GetNameAndRange(nameSheet, statement.MaximumGrateCell));

            var studentsValuesRanges = await
                studentsRangesSheetEditor.GetBatchSheet(ranges, BatchMajorDimensionEnum.COLUMNS);
            
            var students = studentsValuesRanges[0].Values[0];
            var pointsStudents = studentsValuesRanges[1].Values[0];

            if (detailsStatements != null)
            {
                for (int i = 0, step2 = 2; i < detailsStatements.Count; ++i, step2 += 2)
                {
                    var detailsStatement = detailsStatements[i];

                    var details = studentsValuesRanges[step2].Values[0];
                    string maxPoints = studentsValuesRanges[step2 + 1].Values[0][0].ToString();

                    columnsPartsStatements.Add(new ColumnPartStatement(detailsStatement.Title, details, maxPoints));
                }
            }
            
            string maxPointsOverallStudents;
            if (statement.GrateType == GrateType.Sum)
            {
                maxPointsOverallStudents = ParsePointsAndNumber(studentsValuesRanges[^1].Values[0][0].ToString());
            }
            else
            {
                maxPointsOverallStudents = statement.GrateType switch
                {
                    GrateType.Ratio => 1.ToString(),
                    GrateType.Percent => 100.ToString(),
                    GrateType.FiveRatio => 5.ToString(),
                };
            }

            for (int i = 0; i < students.Count && i < pointsStudents.Count; i++)
            {
                if (students.Count < 1 || pointsStudents.Count < 1)
                    continue;
                
                var student = students[i].ToString().Split();
                var pointsStudent = ParsePointsAndNumber(pointsStudents[i].ToString());

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
                    new SubjectGrateData(statement.BlockName, pointsStudent, maxPointsOverallStudents, studentsParts)
                ));
            }
        }

        return studentsGrates.Values.ToList();
    }

    public async Task WriteToSpreadsheet(string spreadsheetLink, List<StudentGratesData> studentsGrates,
        List<StatementSheetData> allStatements, List<SubgroupOfPracticeData> allSubgroups,
        List<TeacherData> allTeachers)
    {
        string GetIdSubgroup(string idSubject, string blockName) => $"{idSubject} - {blockName}";
        
        string nameNewSheet = $"Сводка: {DateTime.Now}";

        var subgroupToIdTeacher = allSubgroups.ToDictionary((subgroup) => subgroup.Id,
            (subgroup) => subgroup.IdTeacher);
        var teachers = allTeachers.ToDictionary((teacher) => teacher.Id);
        var subgroupToTeacher = allSubgroups.ToDictionary((subgroup) => subgroup.Id,
            (subgroup) => teachers[subgroupToIdTeacher[subgroup.Id]]);

        var connectStatementData = new SheetConnectData(GoogleSheetEditor.GetSpreadsheetIdFromLink(spreadsheetLink),
            _sheetConnect.Configuration);
        var sheetService = GoogleSheetEditor.GetSheetsService(connectStatementData);
        var sheetResult = new GoogleSheetEditor(connectStatementData, sheetService, nameNewSheet);

        var statementsGroupBySubjectId = new Dictionary<string, Dictionary<string, bool>>();
        int teachersCount = 0;
        foreach (var statement in allStatements)
        {
            if (statementsGroupBySubjectId.TryGetValue(statement.IdSubject,
                    out var statementsGroupByBlockName) == false)
            {
                statementsGroupByBlockName = new();
                statementsGroupBySubjectId.Add(statement.IdSubject, statementsGroupByBlockName);
            }

            if (statementsGroupByBlockName.TryGetValue(statement.BlockName, out var isOneSubgroup) == false)
            {
                isOneSubgroup = false;
                statementsGroupByBlockName.Add(statement.BlockName, isOneSubgroup);
            }
            else
            {
                teachersCount += !isOneSubgroup ? 1 : 0;
                statementsGroupByBlockName[statement.BlockName] = true;
            }
        }

        string[][] values = new string[studentsGrates.Count + 2][];
        List<MergeCellsRequest> merges = new List<MergeCellsRequest>();
        for (var i = 0; i < values.Length; i++)
        {
            values[i] = new string[statementsGroupBySubjectId.Sum(
                (statementsGroupByBlockName) => statementsGroupByBlockName.Value.Count)
                                   + teachersCount + 1];
        }

        values[0][0] = "ФИО";
        merges.Add(new MergeCellsRequest()
        {
            Range = new GridRange
            {
                StartColumnIndex = 0,
                StartRowIndex = 0,
                EndColumnIndex = 1,
                EndRowIndex = 2,
            },
        });
        for (int i = 0; i < studentsGrates.Count; i++)
        {
            values[i + 2][0] = studentsGrates[i].Student.GetFCs();
        }

        Dictionary<string, int> subgroupToIndex = new();

        {
            int i = 1;
            foreach (var statementsGroupByBlockName in statementsGroupBySubjectId)
            {
                values[0][i] = statementsGroupByBlockName.Key;
                foreach (var statements in statementsGroupByBlockName.Value)
                {
                    values[1][i] = statements.Key;
                    // merges.Add(new MergeCellsRequest()
                    // {
                    //     Range = new GridRange
                    //     {
                    //         StartColumnIndex = i,
                    //         StartRowIndex = 0,
                    //         EndColumnIndex = i + statementsGroupByBlockName.Value.Count - 1,
                    //         EndRowIndex = 1,
                    //     },
                    // });
                    
                    subgroupToIndex.Add(GetIdSubgroup(statementsGroupByBlockName.Key, statements.Key), i);
                    
                    ++i;
                    
                    if (statements.Value)
                    {
                        values[1][i] = "Преподаватели";
                        ++i;
                    }
                }
            }
        }

        for (int i = 0; i < studentsGrates.Count; i++)
        {
            for (int j = 0; j < studentsGrates[i].Subgroups.Count; j++)
            {
                var subgroup = studentsGrates[i].Subgroups[j];
                var index = subgroupToIndex[GetIdSubgroup(subgroup.SubjectId, subgroup.SubjectGrate.Name)];
                
                values[i + 2][index] = (Convert.ToDouble(subgroup.SubjectGrate.CurrentValue) /
                                   Convert.ToDouble(subgroup.SubjectGrate.MaxValue)).ToString("F2");

                if (statementsGroupBySubjectId.TryGetValue(subgroup.SubjectId, out var statementsGroupByBlockName)
                    && statementsGroupByBlockName.TryGetValue(subgroup.SubjectGrate.Name, out bool isOneSubgroup)
                    && isOneSubgroup)
                {
                    values[i + 2][index + 1] = subgroupToTeacher[subgroup.SubgroupId].GetFCs();
                }
            }
        }
        
        List<Request> requests = new List<Request>()
        {
            new Request()
            {
                AddSheet = new AddSheetRequest()
                {
                    Properties = new SheetProperties()
                    {
                        Title = nameNewSheet,
                    },
                },
            },
        };

        await sheetResult.SetSheet(requests);
        await sheetResult.SetSheet(values);

        var sheetId = await GoogleSheetEditor.GetIdBySheetName(sheetService, sheetResult.SpreadsheetId, nameNewSheet);
        var border = new Border
        {
            ColorStyle = new ColorStyle()
            {
                RgbColor = new Color
                {
                    Alpha = 1,
                    Blue = 0,
                    Green = 0,
                    Red = 0,
                },
            },
            Style = "SOLID",
        };
        var overallGridRange = new GridRange
        {
            StartColumnIndex = 0,
            StartRowIndex = 0,
            EndColumnIndex = values[0].Length,
            EndRowIndex = values.Length,
            SheetId = sheetId,
        };
        
        requests.Clear();
        requests.Add(new Request()
        {
            UpdateBorders = new UpdateBordersRequest
            {
                Range = overallGridRange,
                InnerHorizontal = border,
                InnerVertical = border,
                Bottom = border,
                Left = border,
                Right = border,
                Top = border,
            },
        });
        requests.Add(new Request()
        {
            AutoResizeDimensions = new AutoResizeDimensionsRequest
            {
                Dimensions = new DimensionRange
                {
                    Dimension = "COLUMNS",
                    StartIndex = overallGridRange.StartColumnIndex,
                    EndIndex = overallGridRange.EndColumnIndex,
                    SheetId = sheetId,
                },
            },
        });
        
        foreach (var merge in merges)
        {
            merge.Range.SheetId = sheetId;
            merge.MergeType = "MERGE_ALL";
            requests.Add(new Request()
            {
                MergeCells = merge,
            });
        }
        
        await sheetResult.SetSheet(requests);
    }
}