package orazzu.studentmanagerbot.dao;


import com.google.gson.reflect.TypeToken;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import orazzu.studentmanagerbot.Props;
import orazzu.studentmanagerbot.dto.*;
import orazzu.studentmanagerbot.error.StudentManagerException;
import orazzu.studentmanagerbot.model.*;
import orazzu.studentmanagerbot.view.StudentSubjectView;
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
                                        subjectDto.getName(),
                                        null, null, null, null
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
    
    
    public StudentSubjectView getStudentSubject(User user, String subjectId) throws StudentManagerException {
        LOGGER.debug("Getting subject by id {} for {}", subjectId, user);
        
        Request request = new Request.Builder()
                .method("GET", null)
                .url(BASE_URL + "/student/" + user.getTgUsername() + "/subject/" + subjectId)
                .build();
        
        try (Response response = httpClient.newCall(request).execute()) {
            String json = response.body().string();
            
            if (response.isSuccessful()) {
                StudentSubjectDto studentSubjectDto = GSON.fromJson(json, StudentSubjectDto.class);
                final FullSubjectDto subjectDto = studentSubjectDto.getSubject();
                final LecturerDto lecturerDto = subjectDto.getLecturer();
                final SubgroupOfSubjectDto subgroupDto = studentSubjectDto.getSubgroupOfSubject();
                final TeacherDto teacherDto = subgroupDto.getTeacher();
                
                return new StudentSubjectView(
                        new Subject(
                                subjectDto.getId(),
                                subjectDto.getName(),
                                new Lecturer(new Person(
                                        new User(lecturerDto.getTgId(), lecturerDto.getTgUsername()),
                                        lecturerDto.getId(),
                                        lecturerDto.getEmail(),
                                        lecturerDto.getFirstName(),
                                        lecturerDto.getLastName(),
                                        lecturerDto.getPatronymic()
                                )),
                                subjectDto.getGroupId(),
                                subjectDto.getCscLink(),
                                subjectDto.getSemester()
                        ),
                        new SubgroupOfSubject(
                                subgroupDto.getId(),
                                subgroupDto.getLinkToCsc(),
                                new Teacher(new Person(
                                        new User(teacherDto.getTgId(), teacherDto.getTgUsername()),
                                        teacherDto.getId(),
                                        teacherDto.getEmail(),
                                        teacherDto.getFirstName(),
                                        teacherDto.getLastName(),
                                        teacherDto.getPatronymic()
                                ))
                        ),
                        studentSubjectDto.getLinkToLectorStatement(),
                        studentSubjectDto.getLinkToSubgroupStatement()
                );
            }
            
            else {
                LOGGER.info("Failed to get subject by id {} for {}", subjectId, user);
                
                throw toStudentManagerException(json);
            }
        } catch (Exception e) {
            throw wrapAsUnknownError(e);
        }
    }
}
