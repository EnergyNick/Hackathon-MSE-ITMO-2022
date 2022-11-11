package orazzu.studentmanagerbot.model;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class Grade {
    private String name;
    private String currentValue;
    private String maxValue;
    private List<GradePart> parts;
}
