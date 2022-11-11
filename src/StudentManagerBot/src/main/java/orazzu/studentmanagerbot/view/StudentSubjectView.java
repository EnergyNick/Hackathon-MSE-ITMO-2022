package orazzu.studentmanagerbot.view;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import orazzu.studentmanagerbot.model.SubgroupOfSubject;
import orazzu.studentmanagerbot.model.Subject;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class StudentSubjectView {
    private Subject subject;
    private SubgroupOfSubject subgroupOfSubject;
    private String linkToLecturerStatement;
    private String linkToSubgroupStatement;
}
