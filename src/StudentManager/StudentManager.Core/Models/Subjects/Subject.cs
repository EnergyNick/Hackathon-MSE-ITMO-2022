namespace StudentManager.Core.Models.Subjects;

public record Subject : SubjectInfo
{
    public string LectorId { get; init; }
    public string GroupId { get; init; }
    public string CscLink { get; init; }
    public string Semestr { get; init; }
}