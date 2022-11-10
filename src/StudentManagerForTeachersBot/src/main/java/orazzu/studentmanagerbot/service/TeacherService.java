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

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;


public class TeacherService extends ServiceBase {
    private final Logger LOGGER = LoggerFactory.getLogger(TeacherService.class);
    private final StudentManagerDao studentManagerDao;
    private final Map<Long, State> userIdToPayload = new HashMap<>();
    
    
    public TeacherService(StudentManagerDao studentManagerDao) {
        this.studentManagerDao = studentManagerDao;
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> register(Long userId, String username) {
        return List.of(new SendMessage(userId, "Вы зарегистрированы"));
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> postFile(Long userId, String username) {
        return subjectList(userId, username, "file");
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> postLink(Long userId, String username) {
        return subjectList(userId, username, "link");
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> notImplemented(Long userId, Message msg) {
        List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> response = new ArrayList<>();
        
        response.add(new SendMessage(userId, "Ещё не реализовано"));
        
        if (msg != null)
            response.add(deleteButtonsRequest(userId, msg));
        
        return response;
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> processCallback(
            Long userId, String data, Message msg) {
        State state = userIdToPayload.get(userId);
        
        if (userId == null)
            return unknownCallback(userId, msg);
        
        return switch (state.getType()) {
            case POST_LINK -> postLinkAskForSection(userId, data, msg);
            
            default -> unknownCallback(userId, msg);
        };
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> processText(Long userId, String text) {
        State state = userIdToPayload.get(userId);
        
        if (state == null)
            return notCmd(userId);
        
        return switch (state.getType()) {
            case POST_LINK_SUBJECT -> postLinkAskForLink(userId, text);
            case POST_LINK_SUBJECT_SECTION -> postLinkAskForTag(userId, text);
            case POST_LINK_SUBJECT_SECTION_LINK -> postLinkRequest(userId, text);
            
            default -> notCmd(userId);
        };
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
    
    
    private List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> subjectList(
            Long userId, String username, String currCmd) {
        try {
            List<Subject> subjects = studentManagerDao.getSubjectsByUser(new User(userId, username));
            
            if (subjects.isEmpty())
                return List.of(new SendMessage(userId, "У вас нет предметов"));
            
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            subjects.forEach(subject -> keyboard.addRow(new InlineKeyboardButton(subject.getName())
                    .callbackData(subject.getId())
            ));
            
            userIdToPayload.put(userId, new State(StateType.POST_LINK, new ArrayList<>()));
            
            return List.of(new SendMessage(userId, "Выберите предмет")
                    .replyMarkup(keyboard));
        } catch (StudentManagerException e) {
            return errorMessage(userId, e);
        }
    }
    
    
    private List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> postLinkAskForSection(
            Long userId, String subjectId, Message msg) {
        final List<String> payloadData = userIdToPayload.get(userId).getData();
        payloadData.add(subjectId);
        userIdToPayload.put(userId, new State(StateType.POST_LINK_SUBJECT, payloadData));
        
        return List.of(
                new SendMessage(userId, "Введите номер раздела"),
                deleteButtonsRequest(userId, msg)
        );
    }
    
    
    private List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> postLinkAskForLink(
            Long userId, String section) {
        final List<String> payloadData = userIdToPayload.get(userId).getData();
        payloadData.add(section);
        userIdToPayload.put(userId, new State(StateType.POST_LINK_SUBJECT_SECTION, payloadData));
        
        return List.of(new SendMessage(userId, "Введите ссылку"));
    }
    
    
    private List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> postLinkAskForTag(
            Long userId, String link) {
        final List<String> payloadData = userIdToPayload.get(userId).getData();
        payloadData.add(link);
        userIdToPayload.put(userId, new State(StateType.POST_LINK_SUBJECT_SECTION_LINK, payloadData));
        
        return List.of(new SendMessage(userId, "Введите название ссылки"));
    }
    
    
    private List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> postLinkRequest(
            Long userId, String tag) {
        final List<String> payloadData = userIdToPayload.remove(userId).getData();
        
        try {
            studentManagerDao.postLink(
                    payloadData.get(0),
                    payloadData.get(1),
                    payloadData.get(2),
                    tag
            );
            
            return List.of(new SendMessage(userId, "Готово"));
        } catch (StudentManagerException e) {
            return errorMessage(userId, e);
        }
    }
    
    
    private BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse> deleteButtonsRequest(
            Long userId, Message msg) {
        return new EditMessageText(userId, msg.messageId(), msg.text()).replyMarkup(new InlineKeyboardMarkup());
    }
}
