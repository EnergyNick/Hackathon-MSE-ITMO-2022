package orazzu.studentmanagerbot.controller;


import com.pengrad.telegrambot.TelegramBot;
import com.pengrad.telegrambot.UpdatesListener;
import com.pengrad.telegrambot.model.CallbackQuery;
import com.pengrad.telegrambot.model.Message;
import com.pengrad.telegrambot.model.Update;
import com.pengrad.telegrambot.request.BaseRequest;
import com.pengrad.telegrambot.request.SendMessage;
import com.pengrad.telegrambot.response.BaseResponse;
import com.pengrad.telegrambot.response.SendResponse;
import orazzu.studentmanagerbot.service.StudentService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.List;


public class Listener implements UpdatesListener {
    private final Logger LOGGER = LoggerFactory.getLogger(Listener.class);
    private final TelegramBot bot;
    private final StudentService studentService;
    
    
    public Listener(TelegramBot bot, StudentService studentService) {
        this.bot = bot;
        this.studentService = studentService;
    }
    
    
    @Override
    public int process(List<Update> updates) {
        for (Update update: updates) {
            LOGGER.debug("Got update: {}", update);
            
            List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> actions = null;
            
            final Message msg = update.message();
            
            if (msg != null) {
                if (msg.text().startsWith("/"))
                    actions = processCmd(msg);
                
                else
                    actions = processText(msg);
            }
            
            else if (update.callbackQuery() != null) {
                actions = processCallback(update.callbackQuery());
            }
            
            for (BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse> action: actions)
                bot.execute(action);
        }
        
        return UpdatesListener.CONFIRMED_UPDATES_ALL;
    }
    
    
    private List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> processCmd(Message msg) {
        final String text = msg.text();
        final Long userId = msg.chat().id();
        final String username = msg.chat().username();
        
        return switch (text) {
            case "/start" -> studentService.register(userId, username);
            
            case "/get_subject" -> studentService.getSubjects(userId, username);
            
            case "/get_rating" -> studentService.getRating(userId, username);
            
            case "/get_hometasks" -> studentService.getHometasks(userId, username);
            
            default -> studentService.unknownCmd(userId);
        };
    }
    
    
    private List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> processCallback(CallbackQuery query) {
        return studentService.unknownCallback(query.from().id(), query.message());
    }
    
    
    private List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> processText(Message msg) {
        return studentService.notCmd(msg.chat().id());
    }
}
