namespace StudentManager.Tables.Models;

public record AcademicSubjectData : ISheetRowData
{
    public string Id { get; set; }
    public string IdTeacher { get; set; }
    public string IdGroup { get; set; }
    public string Title { get; set; }
    public string LinkToCSC { get; set; }
    public string Term { get; set; }
}