namespace StudentManager.Service.Models.Subjects;

public record SubjectGradeDto
{
    public string Name { get; init; }
    public string CurrentValue { get; init; }
    public string MaxValue { get; init; }
    public List<GradePartDto> Parts { get; init; }
}