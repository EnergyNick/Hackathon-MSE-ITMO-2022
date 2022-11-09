namespace StudentManager.Service.Models.Subjects;

public record UserSubjectInfoDto(SubjectDto Subject, PracticeSubgroupDto? SubgroupOfSubject,
    string? LinkToLectorStatement, string? LinkToSubgroupStatement);