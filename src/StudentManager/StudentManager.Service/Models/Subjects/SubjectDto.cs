namespace StudentManager.Service.Models.Subjects;

public record SubjectDto : SubjectInfoDto
{
    public string LectorId { get; init; }
    public string GroupId { get; init; }
    public string CscLink { get; init; }
    public string Semestr { get; init; }
}