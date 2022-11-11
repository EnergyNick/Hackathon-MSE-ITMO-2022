namespace StudentManager.Service.Models.Subjects;

public record UserSubjectInfoDto(SubjectDto Subject, PracticeSubgroupDto? SubgroupOfSubject,
    string? LinkToLecturerStatement, string? LinkToSubgroupStatement);