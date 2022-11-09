package orazzu.studentmanagerbot;


import com.pengrad.telegrambot.TelegramBot;
import orazzu.studentmanagerbot.controller.Listener;
import orazzu.studentmanagerbot.dao.StudentManagerDao;
import orazzu.studentmanagerbot.service.StudentService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;


public class Main {
    private static final Logger LOGGER = LoggerFactory.getLogger(Main.class);
    
    
    public static void main(String[] args) {
        Props.load();
        
        TelegramBot bot = new TelegramBot(Props.getProp("tg.bot.token"));
        bot.setUpdatesListener(
                new Listener(bot, new StudentService(new StudentManagerDao())),
                e -> LOGGER.error("Connection error", e)
        );
        
        LOGGER.debug("Started");
    }
}
