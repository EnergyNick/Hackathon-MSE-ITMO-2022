namespace StudentManager.Tables.Models;

public enum SessionType
{
    Lecture,
    Practice,
};

public class StatementSheetData : ISheetRowData
{
    public string Id { get; set; }
    public string IdItem { get; set; }
    public SessionType SessionType { get; set; }
    public string IdSubgroup { get; set; }
    public string IdSubject { get; set; }
    public string IdLeafSheet { get; set; }
    public string StudentStartCell { get; set; }
    public string PointsStartCell { get; set; }
}

internal class StatementsSheet
{
    
}
