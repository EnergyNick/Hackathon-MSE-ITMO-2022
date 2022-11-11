namespace StudentManager.Service.Models.Subjects;

public record GradePartDto
{
    public string Name { get; init; }
    public string Value { get; init; }
    public string MaxValue { get; init; }
}