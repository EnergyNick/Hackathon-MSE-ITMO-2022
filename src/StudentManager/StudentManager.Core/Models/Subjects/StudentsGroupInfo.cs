namespace StudentManager.Core.Models.Subjects;

public class StudentsGroupInfo : IBaseEntity<string>
{
    public string Id { get; init; }
    public string Year { get; init; }

    public string? LinkToGoogleGroup { get; set; }
}