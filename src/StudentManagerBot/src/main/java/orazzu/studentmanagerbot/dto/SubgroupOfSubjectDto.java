package orazzu.studentmanagerbot.dto;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class SubgroupOfSubjectDto {
    private String id;
    private String linkToCsc;
    private TeacherDto teacher;
}
