package orazzu.studentmanagerbot.error;


import lombok.AllArgsConstructor;
import lombok.Getter;


@AllArgsConstructor
@Getter
public enum ErrorCode {
    STUDENT_NOT_FOUND("Вы не зарегистрированы в системе"),
    UNKNOWN_ERROR("Неизвестная ошибка");
    
    
    private final String message;
}
