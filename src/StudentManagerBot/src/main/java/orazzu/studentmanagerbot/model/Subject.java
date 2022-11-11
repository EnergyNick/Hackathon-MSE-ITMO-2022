package orazzu.studentmanagerbot.model;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class Subject {
    private String id;
    private String name;
    private Lecturer lecturer;
    private String groupId;
    private String cscLink;
    private String semester;
}
