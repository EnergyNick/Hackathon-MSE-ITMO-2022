package orazzu.studentmanagerbot.service;


import lombok.AllArgsConstructor;
import lombok.Data;

import java.util.List;


@AllArgsConstructor
@Data
public class State {
    private StateType type;
    private List<String> data;
}
