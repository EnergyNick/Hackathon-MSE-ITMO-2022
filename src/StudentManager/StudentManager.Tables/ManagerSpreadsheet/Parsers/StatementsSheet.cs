namespace StudentManager.Tables.Models;

internal class StatementsSheet : BaseGoogleSheetFromRowEditor<StatementSheetData>
{
    protected override Dictionary<string, ColumnCondition<StatementSheetData>> ColumnsDatas { get; } = new ()
    {
        ["ID предмета"] =
            new((data, value) => data.IdSubject = value.ToString(), true),
        ["Тип занятия"] =
            new((data, value) => data.StatementType = ParseStatementType(value), true),
        ["ID подгруппы"] =
            new((data, value) => data.IdSubgroup = value.ToString(), false),
        ["Название блока"] =
            new((data, value) => data.BlockName = value.ToString(), true),
        ["Ссылка на успеваемость с листом"] =
            new((data, value) =>
            {
                string path = value.ToString();
                data.SpreadsheetId = GoogleSheetEditor.GetSpreadsheetIdFromLink(path);
                data.SheetId = Convert.ToInt32(path.Split("#gid=")[1]);
            }, true),
        ["Ячейка начала студентов"] =
            new((data, value) => data.StudentsStartCell = value.ToString(), true),
        ["Ячейка начала успеваемости"] =
            new((data, value) => data.PointsStartCell = value.ToString(), true),
        ["Тип баллов успеваемости"] =
            new((data, value) => data.GrateType = ParseGrateType(value), true),
        ["Ячейка максимального балла"] =
            new((data, value) => data.MaximumGrateCell = value.ToString(), false),
    };

    private static StatementType ParseStatementType(object value)
    {
        return value.ToString() switch
        {
            "Практика" => StatementType.Practice,
            "Лекция" => StatementType.Lecture,
        };
    }
    
    private static string ParseToStatementType(StatementType statementType)
    {
        return statementType switch
        {
            StatementType.Practice => "Практика",
            StatementType.Lecture => "Лекция",
        };
    }


    private static GrateType ParseGrateType(object value)
    {
        return value.ToString() switch
        {
            "Соотношение" => GrateType.Ratio,
            "Проценты" => GrateType.Percent,
            "Патибальная" => GrateType.FiveRatio,
            "Сумма" => GrateType.Sum,
        };
    }

    protected override int SheetId => 1497878140;
    
    public StatementsSheet(SheetConnectData sheetConnectData)
        : base(sheetConnectData)
    {
    }

    protected override void InitAdditionalyParsedData(StatementSheetData data)
    {
        data.Id = $"{data.IdSubgroup} - {ParseToStatementType(data.StatementType)}";
    }
}
