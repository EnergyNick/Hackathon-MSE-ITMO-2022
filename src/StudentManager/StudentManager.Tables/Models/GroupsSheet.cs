namespace StudentManager.Tables.Models;

public record GroupData : ISheetRowData
{
    public string Id { get; set; }
    public string Year { get; set; }
    public string LinkToGoogleGroup { get; set; }
}

internal class GroupsSheet : BaseGoogleSheetFromRowEditor<GroupData>
{
    protected override Dictionary<string, Action<GroupData, object>> InitializeValues => new()
    {
        ["ID"] = (data, value) => data.Id = value.ToString(),
        ["Год"] = (data, value) => data.Year = value.ToString(),
        ["Google группа"] = (data, value) => data.LinkToGoogleGroup = value.ToString(),
    };
    
    protected override string LeafSheet => "Группы";

    public GroupsSheet(SheetConnectData sheetConnectData) : base(sheetConnectData)
    {
    }
}