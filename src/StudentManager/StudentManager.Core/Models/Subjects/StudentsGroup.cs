using StudentManager.Core.Models.Users;

namespace StudentManager.Core.Models.Subjects;

public class StudentsGroup : StudentsGroupInfo
{
    public Student[] Students { get; init; }
}