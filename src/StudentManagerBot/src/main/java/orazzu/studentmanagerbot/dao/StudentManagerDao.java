package orazzu.studentmanagerbot.dao;


import com.google.gson.reflect.TypeToken;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import orazzu.studentmanagerbot.Props;
import orazzu.studentmanagerbot.dto.SubjectDto;
import orazzu.studentmanagerbot.error.StudentManagerException;
import orazzu.studentmanagerbot.model.Subject;
import orazzu.studentmanagerbot.model.User;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ArrayList;
import java.util.List;


public class StudentManagerDao extends DaoBase {
    private final Logger LOGGER = LoggerFactory.getLogger(StudentManagerDao.class);
    private final OkHttpClient httpClient = new OkHttpClient();
    private final String BASE_URL = Props.getProp("student_service_api.url");
    
    
    public List<Subject> getSubjectsByUser(User user) throws StudentManagerException {
        LOGGER.debug("Getting subjects by {}", user);
        
        Request request = new Request.Builder()
                .method("GET", null)
                .url(BASE_URL + "/student/" + user.getTgUsername() + "/subjects")
                .build();
        
        try (Response response = httpClient.newCall(request).execute()) {
            String json = response.body().string();
            
            if (response.isSuccessful()) {
                List<Subject> subjects = new ArrayList<>();
                GSON.fromJson(json, new TypeToken<List<SubjectDto>>() {
                }).forEach(
                        subjectDto -> subjects.add(
                                new Subject(
                                        subjectDto.getId(),
                                        subjectDto.getName()
                                )
                        )
                );
                
                return subjects;
            }
            
            else {
                LOGGER.info("Failed to get subjects by {}: {}", user, response);
                
                throw toStudentManagerException(json);
            }
        } catch (Exception e) {
            throw wrapAsUnknownError(e);
        }
    }
}
