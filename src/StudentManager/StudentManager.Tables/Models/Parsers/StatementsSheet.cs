namespace StudentManager.Tables.Models;

internal class StatementsSheet : BaseGoogleSheetFromRowEditor<StatementSheetData>
{
    protected override Dictionary<string, ColumnCondition<StatementSheetData>> ColumnsDatas { get; } = new ()
    {
        ["ID"] =
            new((data, value) => data.Id = value.ToString(), true),
        ["ID предмета"] =
            new((data, value) => data.IdSubject = value.ToString(), true),
        ["Тип занятия"] =
            new((data, value) => data.StatementType = ParseStatementType(value), true),
        ["ID подгруппы"] =
            new((data, value) => data.IdSubgroup = value.ToString(), false),
        ["ID таблицы ведомости"] =
            new((data, value) => data.IdSheet = value.ToString(), true),
        ["Имя листа"] =
            new((data, value) => data.IdLeafSheet = value.ToString(), true),
        ["Ячейка начала студентов"] =
            new((data, value) => data.StudentsStartCell = value.ToString(), true),
        ["Ячейка начала баллов"] =
            new((data, value) => data.PointsStartCell = value.ToString(), true),
    };

    private static StatementType ParseStatementType(object value)
    {
        return value.ToString() switch
        {
            "Практика" => StatementType.Practice,
            "Лекция" => StatementType.Lecture,
        };
    }

    protected override string LeafSheet => "Ведомости";
    
    public StatementsSheet(SheetConnectData sheetConnectData)
        : base(sheetConnectData)
    {
    }
}
