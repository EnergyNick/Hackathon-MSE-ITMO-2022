package orazzu.studentmanagerbot.dto;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class LecturerDto {
    private Long tgId;
    private String telegramUsername;
    private String id;
    private String email;
    private String firstName;
    private String lastName;
    private String patronymic;
}
