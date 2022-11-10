package orazzu.studentmanagerbot.dto;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import orazzu.studentmanagerbot.model.Lecturer;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class FullSubjectDto {
    private String id;
    private String name;
    private LecturerDto lecturer;
    private String groupId;
    private String cscLink;
    private String semester;
}
