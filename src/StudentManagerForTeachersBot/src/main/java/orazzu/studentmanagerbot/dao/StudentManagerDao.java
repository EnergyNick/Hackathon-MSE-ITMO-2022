package orazzu.studentmanagerbot.dao;


import com.google.gson.reflect.TypeToken;
import okhttp3.*;
import orazzu.studentmanagerbot.Props;
import orazzu.studentmanagerbot.dto.CountTotalGradesDto;
import orazzu.studentmanagerbot.dto.PostLinkDto;
import orazzu.studentmanagerbot.dto.SubjectDto;
import orazzu.studentmanagerbot.error.StudentManagerException;
import orazzu.studentmanagerbot.model.Subject;
import orazzu.studentmanagerbot.model.User;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;


public class StudentManagerDao extends DaoBase {
    private final Logger LOGGER = LoggerFactory.getLogger(StudentManagerDao.class);
    private final OkHttpClient httpClient = new OkHttpClient();
    private final String BASE_URL = Props.getProp("student_manager_api.url");
    
    
    public List<Subject> getSubjectsByUser(User user) throws StudentManagerException {
        LOGGER.debug("Getting subjects by {}", user);
        
        Request request = new Request.Builder()
                .method("GET", null)
                .url(BASE_URL + "/teacher/" + user.getTgUsername() + "/subjects")
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
                LOGGER.info("Failed to get subjects by {}: code={}, body={}", user, response.code(), json);
                
                throw toStudentManagerException(json);
            }
        } catch (IOException | RuntimeException e) {
            throw wrapAsUnknownError(e);
        }
    }
    
    
    public void postLink(String subjectId, String sectionId, String link, String tagName) throws StudentManagerException {
        LOGGER.debug("Posting link {} by subjectId={}, sectionId={}", link, subjectId, sectionId);
        
        Request request = new Request.Builder()
                .method("POST", RequestBody.create(GSON.toJson(new PostLinkDto(tagName, link)), MEDIA_TYPE_JSON))
                .url(BASE_URL + "/teacher/subject/" + subjectId + "/section/" + sectionId + "/attach/link")
                .build();
        
        try (Response response = httpClient.newCall(request).execute()) {
            String body = response.body().string();
            
            if (!response.isSuccessful()) {
                LOGGER.info("Failed to post link {} by subjectId={}, sectionId={}: code={}, body={}",
                        link, subjectId, sectionId, response.code(), body);
                
                throw toStudentManagerException(body);
            }
        } catch (IOException | RuntimeException e) {
            throw wrapAsUnknownError(e);
        }
    }
    
    
    public void countTotalGrades(String username, String link) throws StudentManagerException {
        LOGGER.debug("Counting total grades in {} for user {}", link, username);
        
        Request request = new Request.Builder()
                .method("POST", RequestBody.create(GSON.toJson(new CountTotalGradesDto(link)), MEDIA_TYPE_JSON))
                .url(BASE_URL + "/teacher/" + username + "/subjects/total")
                .build();
        
        try (Response response = httpClient.newCall(request).execute()) {
            String body = response.body().string();
            
            if (!response.isSuccessful()) {
                LOGGER.info("Failed to count total grades in {} for user {}: code={}, body={}",
                        link, username, response.code(), body);
                
                throw toStudentManagerException(body);
            }
        } catch (IOException | RuntimeException e) {
            throw wrapAsUnknownError(e);
        }
    }
}
