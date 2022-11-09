namespace StudentManager.Tables.Models;

public enum StatementType
{
    Practice,
    Lecture,
};

public record StatementSheetData : ISheetRowData
{
    public string Id { get; set; }
    public string IdSubject { get; set; }
    public StatementType StatementType { get; set; }
    public string IdSubgroup { get; set; }
    public string IdSheet { get; set; }
    public string IdLeafSheet { get; set; }
    public string StudentsStartCell { get; set; }
    public string PointsStartCell { get; set; }
}