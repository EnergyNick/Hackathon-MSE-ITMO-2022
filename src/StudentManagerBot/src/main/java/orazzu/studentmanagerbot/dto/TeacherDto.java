package orazzu.studentmanagerbot.dto;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class TeacherDto {
    private Long tgId;
    private String telegramUsername;
    private String id;
    private String email;
    private String firstName;
    private String lastName;
    private String patronymic;
}
