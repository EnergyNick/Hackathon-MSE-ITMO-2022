package orazzu.studentmanagerbot.controller;


import com.pengrad.telegrambot.TelegramBot;
import com.pengrad.telegrambot.UpdatesListener;
import com.pengrad.telegrambot.model.CallbackQuery;
import com.pengrad.telegrambot.model.Message;
import com.pengrad.telegrambot.model.Update;
import com.pengrad.telegrambot.request.BaseRequest;
import com.pengrad.telegrambot.response.BaseResponse;
import orazzu.studentmanagerbot.service.TeacherService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.List;


public class Listener implements UpdatesListener {
    private final Logger LOGGER = LoggerFactory.getLogger(Listener.class);
    private final TelegramBot bot;
    private final TeacherService teacherService;
    
    
    public Listener(TelegramBot bot, TeacherService teacherService) {
        this.bot = bot;
        this.teacherService = teacherService;
    }
    
    
    @Override
    public int process(List<Update> updates) {
        for (Update update: updates) {
            LOGGER.debug("Got update: {}", update);
            
            List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> actions = List.of();
            
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
            case "/start" -> teacherService.register(userId, username);
            
            case "/post_file" -> teacherService.postFile(userId, username);
            
            case "/post_link" -> teacherService.postLink(userId, username);
            
            default -> teacherService.unknownCmd(userId);
        };
    }
    
    
    private List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> processCallback(CallbackQuery query) {
        String[] menuButton = query.data().split(":", 2);
        Long userId = query.from().id();
        String username = query.from().username();
        Message message = query.message();
        
        return switch (menuButton[0]) {
            case "file" -> teacherService.notImplemented(userId);
    
            case "link" -> teacherService.notImplemented(userId);
            
            default -> teacherService.unknownCallback(userId, message);
        };
    }
    
    
    private List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> processText(Message msg) {
        return teacherService.notCmd(msg.chat().id());
    }
}
