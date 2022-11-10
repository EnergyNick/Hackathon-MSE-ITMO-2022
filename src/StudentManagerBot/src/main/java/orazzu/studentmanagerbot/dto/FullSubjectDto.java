package orazzu.studentmanagerbot.dto;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;


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
