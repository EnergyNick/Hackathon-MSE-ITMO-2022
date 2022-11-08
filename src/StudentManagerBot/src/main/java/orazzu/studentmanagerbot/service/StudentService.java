package orazzu.studentmanagerbot.service;


import com.pengrad.telegrambot.model.Message;
import com.pengrad.telegrambot.model.request.InlineKeyboardButton;
import com.pengrad.telegrambot.model.request.InlineKeyboardMarkup;
import com.pengrad.telegrambot.model.request.ReplyKeyboardRemove;
import com.pengrad.telegrambot.request.BaseRequest;
import com.pengrad.telegrambot.request.EditMessageReplyMarkup;
import com.pengrad.telegrambot.request.EditMessageText;
import com.pengrad.telegrambot.request.SendMessage;
import com.pengrad.telegrambot.response.BaseResponse;
import com.pengrad.telegrambot.response.SendResponse;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.List;


public class StudentService {
    private final Logger LOGGER = LoggerFactory.getLogger(StudentService.class);
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> register(Long userId, String username) {
        return List.of(new SendMessage(userId, "Вы зарегистрированы"));
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> getSubjects(Long userId, String username) {
        return List.of(new SendMessage(userId, "Выберите предмет")
                .replyMarkup(new InlineKeyboardMarkup(
                                new InlineKeyboardButton("<Название предмета 1>").callbackData("1"),
                                new InlineKeyboardButton("<Название предмета 2>").callbackData("2")
                        )
                ));
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> getRating(Long userId, String username) {
        return List.of(new SendMessage(userId, "Выберите предмет")
                .replyMarkup(new InlineKeyboardMarkup(
                                new InlineKeyboardButton("<Название предмета 1>").callbackData("1"),
                                new InlineKeyboardButton("<Название предмета 2>").callbackData("2")
                        )
                ));
    }
    
    
    public List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> getHometasks(Long userId, String username) {
        return List.of(new SendMessage(userId, "Выберите предмет")
                .replyMarkup(new InlineKeyboardMarkup(
                                new InlineKeyboardButton("<Название предмета 1>").callbackData("1"),
                                new InlineKeyboardButton("<Название предмета 2>").callbackData("2")
                        )
                ));
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
}
