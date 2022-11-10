using StudentManager.Service.Models.Users;

namespace StudentManager.Service.Models.Subjects;

public record PracticeSubgroupDto
{
    public string Id { get; init; }
    public string LinkToCSC { get; init; }
    public TeacherDto Teacher { get; set; }
    public string LinkForHomerworks { get; set; }
}