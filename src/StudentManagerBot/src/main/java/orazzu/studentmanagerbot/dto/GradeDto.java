package orazzu.studentmanagerbot.dto;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class GradeDto {
    private String name;
    private String currentValue;
    private String maxValue;
    private List<GradePartDto> parts;
}
