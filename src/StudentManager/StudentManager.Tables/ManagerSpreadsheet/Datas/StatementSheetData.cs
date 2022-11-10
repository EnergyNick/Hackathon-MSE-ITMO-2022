namespace StudentManager.Tables.Models;

public enum StatementType
{
    Practice,
    Lecture,
};

public enum GrateType
{
    Ratio,
    Percent,
    Sum,
};

public record StatementSheetData : ISheetRowData
{
    public string Id { get; set; }
    public string IdSubject { get; set; }
    public StatementType StatementType { get; set; }
    public string IdSubgroup { get; set; }
    public string BlockName { get; set; }
    public string SpreadsheetId { get; set; }
    public int SheetId { get; set; }
    public string StudentsStartCell { get; set; }
    public string PointsStartCell { get; set; }
    public GrateType GrateType { get; set; }
    public string MaximumGrateCell { get; set; }
}