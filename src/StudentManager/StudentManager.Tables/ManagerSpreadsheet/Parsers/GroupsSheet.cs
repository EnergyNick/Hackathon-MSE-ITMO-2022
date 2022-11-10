namespace StudentManager.Tables.Models;

internal class GroupsSheet : BaseGoogleSheetFromRowEditor<GroupData>
{
    protected override Dictionary<string, ColumnCondition<GroupData>> ColumnsDatas { get; } = new()
    {
        ["ID"] =
            new ((data, value) => data.Id = value.ToString(), true),
        ["Год"] =
            new((data, value) => data.Year = value.ToString(), true),
        ["Google группа"] =
            new((data, value) => data.LinkToGoogleGroup = value.ToString(), true),
    };
    
    protected override int SheetId => 982408780;

    public GroupsSheet(SheetConnectData sheetConnectData) : base(sheetConnectData)
    {
    }
}