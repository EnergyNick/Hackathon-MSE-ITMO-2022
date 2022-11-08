namespace StudentManager.Tables.Models;

public record GroupData : ISheetRowData
{
    public string Id { get; set; }
    public string Year { get; set; }
    public string LinkToGoogleGroup { get; set; }
}

internal class GroupsSheet : GoogleSheetFromRowEditor<GroupData>
{
    protected override Dictionary<string, Action<GroupData, object>> InitializeValues { get; }

    public GroupsSheet(SheetConnectData connectData)
        : base(new GoogleSheetEditor(connectData, "Группы"))
    {
    }
}