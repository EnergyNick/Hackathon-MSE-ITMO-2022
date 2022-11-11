package orazzu.studentmanagerbot.view;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import orazzu.studentmanagerbot.model.Grade;

import java.util.List;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class StudentSubjectGradesView {
    private String subjectName;
    private List<Grade> grades;
    private String linkToLecturerStatement;
    private String linkToSubgroupStatement;
}
