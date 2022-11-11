namespace StudentManager.Core.Models.Subjects;

public record PracticeSubgroup : IBaseEntity<string>
{
    public string Id { get; init; }
    public string SubjectId { get; init; }
    public string TeacherId { get; init; }
    public string LinkToCSC { get; init; }
}