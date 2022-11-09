namespace StudentManager.Tables.Models;

public record SubgroupOfPracticeData : ISheetRowData
{
    public string Id { get; set; }
    public string IdSubject { get; set; }
    public string IdTeacher { get; set; }
    public string LinkToCSC { get; set; }
}