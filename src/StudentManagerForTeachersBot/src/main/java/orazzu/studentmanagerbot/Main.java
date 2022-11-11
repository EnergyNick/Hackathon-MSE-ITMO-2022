package orazzu.studentmanagerbot;


import com.pengrad.telegrambot.TelegramBot;
import orazzu.studentmanagerbot.controller.Listener;
import orazzu.studentmanagerbot.dao.StudentManagerDao;
import orazzu.studentmanagerbot.service.TeacherService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;


public class Main {
    private static final Logger LOGGER = LoggerFactory.getLogger(Main.class);
    
    
    public static void main(String[] args) {
        Props.load(args.length >= 1? args[0] : "bot.properties");
        
        TelegramBot bot = new TelegramBot(Props.getProp("tg.bot.token"));
        bot.setUpdatesListener(
                new Listener(bot, new TeacherService(new StudentManagerDao())),
                e -> LOGGER.error("Connection error", e)
        );
        
        LOGGER.debug("Started");
    }
}
