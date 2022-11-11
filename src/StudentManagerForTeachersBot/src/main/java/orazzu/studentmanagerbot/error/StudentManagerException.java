package orazzu.studentmanagerbot.error;


import lombok.Getter;
import lombok.RequiredArgsConstructor;


@RequiredArgsConstructor
@Getter
public class StudentManagerException extends Exception {
    private final ErrorCode errorCode;
}
