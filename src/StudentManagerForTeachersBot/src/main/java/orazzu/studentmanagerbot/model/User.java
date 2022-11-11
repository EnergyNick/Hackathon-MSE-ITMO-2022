package orazzu.studentmanagerbot.model;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class User {
    private Long tgId;
    private String tgUsername;
}
