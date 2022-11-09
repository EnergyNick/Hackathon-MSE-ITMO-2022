namespace StudentManager.Tables.Models;

public record DetailsStatementData : ISheetRowData
{
    public string Id { get; set; }
    public string IdStatement { get; set; }
    public string Title { get; set; }
    public string PointsStartCell { get; set; }
};

internal class DetailsStatementsSheet : BaseGoogleSheetFromRowEditor<DetailsStatementData>
{
    protected override Dictionary<string, ColumnCondition<DetailsStatementData>> ColumnsDatas { get; } = new()
    {
        ["ID"] =
            new((data, value) => data.Id = value.ToString(), true),
        ["ID ведомости"] =
            new((data, value) => data.IdStatement = value.ToString(), true),
        ["Название"] =
            new((data, value) => data.Title = value.ToString(), false),
        ["Ячейка начала баллов"] =
            new((data, value) => data.PointsStartCell = value.ToString(), true),
    };

    protected override string LeafSheet => "Детали ведомостей";
    
    public DetailsStatementsSheet(SheetConnectData sheetConnectData) : base(sheetConnectData)
    {
    }
}