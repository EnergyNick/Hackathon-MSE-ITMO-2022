package orazzu.studentmanagerbot.dto;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class StudentSubjectDto {
    private FullSubjectDto subject;
    private SubgroupOfSubjectDto subgroupOfSubject;
    private String linkToLectorStatement;
    private String linkToSubgroupStatement;
}
