package orazzu.studentmanagerbot.service;


import com.pengrad.telegrambot.model.Message;
import com.pengrad.telegrambot.model.request.InlineKeyboardButton;
import com.pengrad.telegrambot.model.request.InlineKeyboardMarkup;
import com.pengrad.telegrambot.request.BaseRequest;
import com.pengrad.telegrambot.request.EditMessageText;
import com.pengrad.telegrambot.request.SendMessage;
import com.pengrad.telegrambot.response.BaseResponse;
import orazzu.studentmanagerbot.dao.StudentManagerDao;
import orazzu.studentmanagerbot.error.StudentManagerException;
import orazzu.studentmanagerbot.model.Subject;
import orazzu.studentmanagerbot.model.User;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.List;


public class StudentService extends ServiceBase {
    private final Logger LOGGER = LoggerFactory.getLogger(StudentService.class);
    private final StudentManagerDao studentManagerDao;
    
    
    public StudentService(StudentManagerDao studentManagerDao) {
        this.studentManagerDao = studentManagerDao;
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> register(Long userId, String username) {
        return List.of(new SendMessage(userId, "Вы зарегистрированы"));
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> getSubjects(Long userId, String username) {
        return subjectList(userId, username, "subjects");
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> getRating(Long userId, String username) {
        return subjectList(userId, username, "rating");
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> getHometasks(Long userId, String username) {
        return subjectList(userId, username, "hometasks");
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> unknownCmd(Long userId) {
        return List.of(new SendMessage(userId, "Неизвестная команда"));
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> unknownCallback(Long userId, Message callbackMsg) {
        return List.of(
                new SendMessage(userId, "Ой, во что это вы тыкнули?.."),
                new EditMessageText(userId, callbackMsg.messageId(), callbackMsg.text()).replyMarkup(new InlineKeyboardMarkup())
        );
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> notCmd(Long userId) {
        return List.of(new SendMessage(userId, "Я понимаю только команды"));
    }
    
    
    private List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> subjectList(Long userId, String username, String callbackPrefix) {
        try {
            List<Subject> subjects = studentManagerDao.getSubjectsByUser(new User(userId, username));
            
            if (subjects.isEmpty())
                return List.of(new SendMessage(userId, "У вас нет предметов"));
            
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            subjects.forEach(subject -> keyboard.addRow(new InlineKeyboardButton(subject.getName())
                    .callbackData(callbackPrefix + ":" + subject.getId())
            ));
            
            return List.of(new SendMessage(userId, "Выберите предмет")
                    .replyMarkup(keyboard));
        } catch (StudentManagerException e) {
            return errorMessage(userId, e);
        }
    }
}