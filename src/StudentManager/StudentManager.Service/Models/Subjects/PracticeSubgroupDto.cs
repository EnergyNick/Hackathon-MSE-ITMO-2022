namespace StudentManager.Service.Models.Subjects;

public record PracticeSubgroupDto
{
    public string Id { get; init; }
    public string SubjectId { get; init; }
    public string TeacherId { get; init; }
    public string LinkToCSC { get; init; }
}