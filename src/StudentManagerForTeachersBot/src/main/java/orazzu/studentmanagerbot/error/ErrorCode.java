package orazzu.studentmanagerbot.error;


import lombok.AllArgsConstructor;
import lombok.Getter;


@AllArgsConstructor
@Getter
public enum ErrorCode {
    ELEMENT_NOT_FOUND_BY_ID("Элемент не найден"),
    USER_NOT_FOUND_BY_TELEGRAM_USERNAME("Вы не зарегистрированы в системе"),
    CANT_FIND_GRADES_BY_USER_ID("Не удалось найти оценки для пользователя"),
    ERROR_ON_PARSING_TABLES("Ошибка доступа к таблицам"),
    UNKNOWN_ERROR("Неизвестная ошибка");
    
    
    private final String message;
}
