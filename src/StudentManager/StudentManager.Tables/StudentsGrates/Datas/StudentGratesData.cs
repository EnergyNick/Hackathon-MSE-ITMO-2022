namespace StudentManager.Tables.Models;

public record StudentGratesData(StudentData Student, List<SubgroupOfStudentData> Subgroups);

public record SubgroupOfStudentData(string SubjectId, string? SubgroupId, SubjectGrateData SubjectGrate);
public record SubjectGrateData(string Name, string CurrentValue, string MaxValue, List<GratePartData> Parts);
public record GratePartData(string Name, string Value, string MaxValue);