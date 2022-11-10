package orazzu.studentmanagerbot.service;


import com.pengrad.telegrambot.model.Message;
import com.pengrad.telegrambot.model.request.InlineKeyboardMarkup;
import com.pengrad.telegrambot.request.BaseRequest;
import com.pengrad.telegrambot.request.EditMessageText;
import com.pengrad.telegrambot.request.SendMessage;
import com.pengrad.telegrambot.response.BaseResponse;
import orazzu.studentmanagerbot.dao.StudentManagerDao;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.List;


public class TeacherService extends ServiceBase {
    private final Logger LOGGER = LoggerFactory.getLogger(TeacherService.class);
    private final StudentManagerDao studentManagerDao;
    
    
    public TeacherService(StudentManagerDao studentManagerDao) {
        this.studentManagerDao = studentManagerDao;
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> register(Long userId, String username) {
        return List.of(new SendMessage(userId, "Вы зарегистрированы"));
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> notImplemented(Long userId) {
        return List.of(new SendMessage(userId, "Ещё не реализовано"));
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> unknownCmd(Long userId) {
        return List.of(new SendMessage(userId, "Неизвестная команда"));
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> unknownCallback(
            Long userId, Message msg) {
        return List.of(
                new SendMessage(userId, "Ой, во что это вы тыкнули?.."),
                deleteButtonsRequest(userId, msg)
        );
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> notCmd(Long userId) {
        return List.of(new SendMessage(userId, "Я понимаю только команды"));
    }
    
    
    private BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse> deleteButtonsRequest(
            Long userId, Message msg) {
        return new EditMessageText(userId, msg.messageId(), msg.text()).replyMarkup(new InlineKeyboardMarkup());
    }
}
