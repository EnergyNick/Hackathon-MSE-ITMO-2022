namespace StudentManager.Tables.Models;

public record GroupData(string Id, string Year, string LinkToGoogleGroup);

internal class GroupsTable : BaseConnectToSheet<GroupData>
{
    public GroupsTable(SheetConnectData connectData)
        : base(new GoogleSheetsEditor(connectData, "Группы"))
    {
    }

    protected override List<GroupData> Parse(IList<IList<object>> sheetValues, IList<AvailableColumn> availableColumns)
    {
        throw new NotImplementedException();
    }

    protected override bool ContainsColumn(string nameColumn)
    {
        throw new NotImplementedException();
    }
}