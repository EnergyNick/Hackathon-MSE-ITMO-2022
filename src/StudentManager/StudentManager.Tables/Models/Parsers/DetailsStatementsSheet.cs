namespace StudentManager.Tables.Models;

public record DetailsStatementData : ISheetRowData
{
    public string Id { get; set; }
    public string IdStatement { get; set; }
    public string Title { get; set; }
    public string PointsStartCell { get; set; }
    public string MaximumGrateCell { get; set; }
};

internal class DetailsStatementsSheet : BaseGoogleSheetFromRowEditor<DetailsStatementData>
{
    protected override Dictionary<string, ColumnCondition<DetailsStatementData>> ColumnsDatas { get; } = new()
    {
        ["ID ведомости"] =
            new((data, value) => data.IdStatement = value.ToString(), true),
        ["Название"] =
            new((data, value) => data.Title = value.ToString(), false),
        ["Ячейка начала баллов"] =
            new((data, value) => data.PointsStartCell = value.ToString(), true),
        ["Ячейка максимального балла"] =
            new((data, value) => data.MaximumGrateCell = value.ToString(), false),
    };

    protected override int SheetId => 845033275;
    
    public DetailsStatementsSheet(SheetConnectData sheetConnectData) : base(sheetConnectData)
    {
    }

    protected override void InitAdditionalyParsedData(DetailsStatementData data)
    {
        data.Id = $"{data.IdStatement} - {data.Title}";
    }
}