package orazzu.studentmanagerbot.dto;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class StudentSubjectGradesDto {
    private String subjectName;
    private List<GradeDto> grades;
    private String linkToLecturerStatement;
    private String linkToSubgroupStatement;
}
