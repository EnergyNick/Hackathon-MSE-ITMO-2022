using StudentManager.Service.Models.Users;

namespace StudentManager.Service.Models.Subjects;

public class StudentsGroupDto : StudentsGroupInfoDto
{
    public StudentDto[] Students { get; init; }
}