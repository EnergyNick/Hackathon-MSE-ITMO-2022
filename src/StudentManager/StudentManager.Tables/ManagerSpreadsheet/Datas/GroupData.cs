namespace StudentManager.Tables.Models;

public record GroupData : ISheetRowData
{
    public string Id { get; set; }
    public string Year { get; set; }
    public string LinkToGoogleGroup { get; set; }
}