package orazzu.studentmanagerbot.service;


import com.pengrad.telegrambot.request.BaseRequest;
import com.pengrad.telegrambot.request.SendMessage;
import com.pengrad.telegrambot.response.SendResponse;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;


public class StudentService {
    private final Logger LOGGER = LoggerFactory.getLogger(StudentService.class);
    
    
    public BaseRequest<SendMessage, SendResponse> register(Long userId, String userName) {
        return new SendMessage(userId, "Вы зарегистрированы");
    }
    
    
    public BaseRequest<SendMessage, SendResponse> unknownCmd(Long userId) {
        return new SendMessage(userId, "Неизвестная команда");
    }
    
    
    public BaseRequest<SendMessage, SendResponse> notCmd(Long userId) {
        return new SendMessage(userId, "Я понимаю только команды");
    }
}
