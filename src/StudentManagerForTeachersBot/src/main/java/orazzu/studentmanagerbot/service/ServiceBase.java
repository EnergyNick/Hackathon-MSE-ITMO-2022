package orazzu.studentmanagerbot.service;


import com.pengrad.telegrambot.request.BaseRequest;
import com.pengrad.telegrambot.request.SendMessage;
import com.pengrad.telegrambot.response.BaseResponse;
import orazzu.studentmanagerbot.error.StudentManagerException;

import java.util.List;


public class ServiceBase {
    protected List<BaseRequest<? extends BaseRequest<?, ?>, ? extends BaseResponse>> errorMessage(
            Long userId, StudentManagerException e) {
        return List.of(new SendMessage(userId, e.getErrorCode().getMessage()));
    }
}
