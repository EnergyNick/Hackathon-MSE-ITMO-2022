package orazzu.studentmanagerbot.service.util;


import com.pengrad.telegrambot.model.MessageEntity;
import com.pengrad.telegrambot.request.SendMessage;

import java.util.ArrayList;
import java.util.List;


public class MessageBuilder {
    private final Long userId;
    private final StringBuilder msgBuilder = new StringBuilder();
    private final List<MessageEntity> msgFormatters;
    private Integer len = 0;
    
    
    public MessageBuilder(Long userId) {
        this.userId = userId;
        this.msgFormatters = new ArrayList<>();
    }
    
    
    public MessageBuilder appendLine(String text) {
        if (!msgBuilder.isEmpty()) {
            msgBuilder.append('\n');
            len++;
        }
        
        if (text != null) {
            msgBuilder.append(text);
            len += text.length();
        }
        
        return this;
    }
    
    
    public MessageBuilder appendLine() {
        return appendLine(null);
    }
    
    
    public MessageBuilder appendKeyValue(String key, String value) {
        if (value == null || value.equals("-"))
            return this;
        
        return appendLine(key + ": " + value);
    }
    
    
    public MessageBuilder appendKeyValueTg(String key, String tgUsername) {
        return appendKeyValue(key, "@" + tgUsername);
    }
    
    
    public MessageBuilder appendHeader(String text) {
        if (!msgBuilder.isEmpty()) {
            msgBuilder.append("\n\n");
            len += 2;
        }
        
        if (text != null) {
            msgBuilder.append(text);
            
            final int textLen = text.length();
            msgFormatters.add(new MessageEntity(MessageEntity.Type.bold, len, textLen));
            len += textLen;
        }
        
        return this;
    }
    
    
    public SendMessage build() {
        return new SendMessage(userId, msgBuilder.toString()).entities(msgFormatters.toArray(new MessageEntity[]{}));
    }
}
