namespace StudentManager.Core.Models.Subjects;

public record Statement : IBaseEntity<string>
{
    public string Id { get; init; }
    public string SubjectId { get; set; }
    public StatementType Type { get; set; }
    public string SubgroupId { get; set; }

    public string SheetId { get; set; }
    public string SheetLeafId { get; set; }

    public string StudentsStartCell { get; set; }
    public string PointsStartCell { get; set; }
}