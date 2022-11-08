package orazzu.studentmanagerbot;


import com.pengrad.telegrambot.TelegramBot;
import orazzu.studentmanagerbot.controller.Listener;
import orazzu.studentmanagerbot.service.StudentService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;


public class Main {
    private static final Logger LOGGER = LoggerFactory.getLogger(Main.class);
    private static final Properties PROPS = new Properties();
    
    
    public static void main(String[] args) {
        try (InputStream propsStream = new FileInputStream("bot.properties")) {
            PROPS.load(propsStream);
        } catch (IOException e) {
            LOGGER.error("Failed to read props", e);
            return;
        }
        
        TelegramBot bot = new TelegramBot(PROPS.getProperty("tg.bot.token"));
        
        bot.setUpdatesListener(new Listener(bot, new StudentService()), e -> LOGGER.error("Connection error", e));
        
        LOGGER.debug("Started");
    }
}
