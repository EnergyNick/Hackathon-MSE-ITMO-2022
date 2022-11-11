package orazzu.studentmanagerbot.model;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class GradePart {
    private String name;
    private String value;
    private String maxValue;
}
