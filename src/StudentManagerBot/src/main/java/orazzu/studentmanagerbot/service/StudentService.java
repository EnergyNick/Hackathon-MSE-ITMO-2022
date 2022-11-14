package orazzu.studentmanagerbot.service;


import com.pengrad.telegrambot.model.Message;
import com.pengrad.telegrambot.model.request.InlineKeyboardButton;
import com.pengrad.telegrambot.model.request.InlineKeyboardMarkup;
import com.pengrad.telegrambot.request.BaseRequest;
import com.pengrad.telegrambot.request.EditMessageText;
import com.pengrad.telegrambot.request.SendMessage;
import com.pengrad.telegrambot.response.BaseResponse;
import orazzu.studentmanagerbot.dao.StudentManagerDao;
import orazzu.studentmanagerbot.error.ErrorCode;
import orazzu.studentmanagerbot.error.StudentManagerException;
import orazzu.studentmanagerbot.model.Subject;
import orazzu.studentmanagerbot.model.User;
import orazzu.studentmanagerbot.service.util.EntityToMessageMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;


public class StudentService extends ServiceBase {
    private final Logger LOGGER = LoggerFactory.getLogger(StudentService.class);
    private final StudentManagerDao studentManagerDao;
    private final Map<Long, State> userIdToState = new HashMap<>();
    
    
    public StudentService(StudentManagerDao studentManagerDao) {
        this.studentManagerDao = studentManagerDao;
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> register(Long userId, String username) {
        return List.of(new SendMessage(userId, "Вы зарегистрированы"));
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> getSubjects(
            Long userId, String username) {
        return subjectList(userId, username, StateType.GET_SUBJECT);
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> getGrades(
            Long userId, String username) {
        return subjectList(userId, username, StateType.GET_GRADES);
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> getHometasks(
            Long userId, String username) {
        return subjectList(userId, username, StateType.GET_HOMETASKS);
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> unknownCmd(Long userId) {
        return List.of(new SendMessage(userId, "Неизвестная команда"));
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> getStudentSubjectCallback(
            Long userId, String username, String subjectId, Message msg) {
        try {
            userIdToState.remove(userId);
            
            return List.of(
                    EntityToMessageMapper.map(
                            userId,
                            studentManagerDao.getStudentSubjectId(new User(userId, username), subjectId)
                    ),
                    deleteButtonsRequest(userId, msg)
            );
        } catch (StudentManagerException e) {
            return errorMessage(userId, new StudentManagerException(ErrorCode.UNKNOWN_ERROR));
        }
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> getStudentGradesBySubjectCallback(
            Long userId, String username, String subjectId, Message msg) {
        try {
            userIdToState.remove(userId);
            
            return List.of(
                    EntityToMessageMapper.map(
                            userId,
                            studentManagerDao.getStudentGradesBySubjectId(new User(userId, username), subjectId)
                    ),
                    deleteButtonsRequest(userId, msg)
            );
        } catch (StudentManagerException e) {
            return errorMessage(userId, new StudentManagerException(ErrorCode.UNKNOWN_ERROR));
        }
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> unknownCallback(
            Long userId, Message msg) {
        return List.of(
                new SendMessage(userId, "Ой, во что это вы тыкнули?.."),
                deleteButtonsRequest(userId, msg)
        );
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> notImplemented(Long userId, Message msg) {
        List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> response = new ArrayList<>();
        
        response.add(new SendMessage(userId, "Ещё не реализовано"));
        
        if (msg != null)
            response.add(deleteButtonsRequest(userId, msg));
        
        return response;
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> processCallback(
            Long userId, String username, String data, Message msg) {
        State state = userIdToState.get(userId);
        
        if (state == null)
            return unknownCallback(userId, msg);
        
        return switch (state.getType()) {
            case GET_SUBJECT -> getStudentSubjectCallback(userId, username, data, msg);
            case GET_GRADES -> getStudentGradesBySubjectCallback(userId, username, data, msg);
            case GET_HOMETASKS -> notImplemented(userId, msg);
        };
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> processText(Long userId, String text) {
        return notCmd(userId);
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> notCmd(Long userId) {
        return List.of(new SendMessage(userId, "Я понимаю только команды"));
    }
    
    
    private List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> subjectList(
            Long userId, String username, StateType stateType) {
        try {
            List<Subject> subjects = studentManagerDao.getSubjectsByUser(new User(userId, username));
            
            if (subjects.isEmpty())
                return List.of(new SendMessage(userId, "У вас нет предметов"));
            
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            subjects.forEach(subject -> keyboard.addRow(new InlineKeyboardButton(subject.getName())
                    .callbackData(subject.getId())
            ));
            
            userIdToState.put(userId, new State(stateType, new ArrayList<>()));
            
            return List.of(new SendMessage(userId, "Выберите предмет")
                    .replyMarkup(keyboard));
        } catch (StudentManagerException e) {
            return errorMessage(userId, e);
        }
    }
    
    
    private BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse> deleteButtonsRequest(
            Long userId, Message msg) {
        return new EditMessageText(userId, msg.messageId(), msg.text()).replyMarkup(new InlineKeyboardMarkup());
    }
}
