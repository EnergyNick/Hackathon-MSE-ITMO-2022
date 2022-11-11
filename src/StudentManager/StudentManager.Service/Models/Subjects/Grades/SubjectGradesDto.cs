namespace StudentManager.Service.Models.Subjects;

public record SubjectGradesDto(string SubjectName, SubjectGradeDto[] Grades,
    string? LinkToLecturerStatement, string? LinkToSubgroupStatement);