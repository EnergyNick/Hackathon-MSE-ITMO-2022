package orazzu.studentmanagerbot.model;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class SubgroupOfSubject {
    private String id;
    private String linkToCsc;
    private Teacher teacher;
}
