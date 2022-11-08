package orazzu.studentmanagerbot.controller;


import com.pengrad.telegrambot.TelegramBot;
import com.pengrad.telegrambot.UpdatesListener;
import com.pengrad.telegrambot.model.Update;
import com.pengrad.telegrambot.request.BaseRequest;
import com.pengrad.telegrambot.request.SendMessage;
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
            
            Long userId = update.message().chat().id();
            BaseRequest<SendMessage, SendResponse> answer = null;
            
            final String text = update.message().text();
            if (text != null) {
                if (text.startsWith("/")) {
                    answer = switch (text) {
                        case "/start" -> studentService.register(
                                userId,
                                update.message().chat().username()
                        );
                        
                        default -> studentService.unknownCmd(userId);
                    };
                }
                
                else
                    answer = studentService.notCmd(userId);
                
                bot.execute(answer);
            }
        }
        
        return UpdatesListener.CONFIRMED_UPDATES_ALL;
    }
}
