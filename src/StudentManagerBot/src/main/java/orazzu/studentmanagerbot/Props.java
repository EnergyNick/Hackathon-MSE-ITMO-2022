package orazzu.studentmanagerbot;


import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;


public class Props {
    private static final Logger LOGGER = LoggerFactory.getLogger(Props.class);
    private static final Properties PROPS = new Properties();
    
    
    public static void load() {
        if (PROPS.isEmpty())
            try (InputStream propsStream = new FileInputStream("bot.properties")) {
                PROPS.load(propsStream);
            } catch (IOException e) {
                LOGGER.error("Failed to read props", e);
                System.exit(1);
            }
    }
    
    
    public static String getProp(String key) {
        return PROPS.getProperty(key);
    }
}
