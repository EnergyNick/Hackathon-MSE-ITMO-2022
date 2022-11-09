using StudentManager.Service.Models.Users;

namespace StudentManager.Service.Models.Subjects;

public record SubjectDto : SubjectInfoDto
{
    public TeacherDto Lector { get; set; }
    public string GroupId { get; init; }
    public string CscLink { get; init; }
    public string Semestr { get; init; }
}