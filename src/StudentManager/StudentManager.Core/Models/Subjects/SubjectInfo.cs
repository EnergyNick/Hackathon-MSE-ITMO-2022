namespace StudentManager.Core.Models.Subjects;

public record SubjectInfo : IBaseEntity<string>
{
    public string Id { get; init; }
    public string Name { get; init; }
}