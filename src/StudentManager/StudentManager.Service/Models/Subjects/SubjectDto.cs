using StudentManager.Service.Models.Users;

namespace StudentManager.Service.Models.Subjects;

public record SubjectDto : SubjectInfoDto
{
    public TeacherDto Lecturer { get; set; }
    public string GroupId { get; init; }
    public string CscLink { get; init; }
    public string Semester { get; init; }
}