package orazzu.studentmanagerbot.dao;


import com.google.gson.reflect.TypeToken;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import orazzu.studentmanagerbot.Props;
import orazzu.studentmanagerbot.dto.*;
import orazzu.studentmanagerbot.error.StudentManagerException;
import orazzu.studentmanagerbot.model.*;
import orazzu.studentmanagerbot.view.StudentSubjectGradesView;
import orazzu.studentmanagerbot.view.StudentSubjectView;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;


public class StudentManagerDao extends DaoBase {
    private final Logger LOGGER = LoggerFactory.getLogger(StudentManagerDao.class);
    private final OkHttpClient httpClient = new OkHttpClient();
    private final String BASE_URL = Props.getProp("student_manager_api.url");
    
    
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
                LOGGER.info("Failed to get subjects by {}: code={}, body={}", user, response.code(), json);
                
                throw toStudentManagerException(json);
            }
        } catch (IOException | RuntimeException e) {
            throw wrapAsUnknownError(e);
        }
    }
    
    
    public StudentSubjectView getStudentSubjectId(User user, String subjectId) throws StudentManagerException {
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
                final TeacherDto teacherDto = subgroupDto != null? subgroupDto.getTeacher() : null;
                
                return new StudentSubjectView(
                        subjectDto != null?
                                new Subject(
                                        subjectDto.getId(),
                                        subjectDto.getName(),
                                        lecturerDto != null?
                                                new Lecturer(new Person(
                                                        new User(lecturerDto.getTgId(), lecturerDto.getTelegramUsername()),
                                                        lecturerDto.getId(),
                                                        lecturerDto.getEmail(),
                                                        lecturerDto.getFirstName(),
                                                        lecturerDto.getLastName(),
                                                        lecturerDto.getPatronymic()
                                                )) : null,
                                        subjectDto.getGroupId(),
                                        subjectDto.getCscLink(),
                                        subjectDto.getSemester()
                                ) : null,
                        subgroupDto != null?
                                new SubgroupOfSubject(
                                        subgroupDto.getId(),
                                        subgroupDto.getLinkToCsc(),
                                        teacherDto != null?
                                                new Teacher(new Person(
                                                        new User(teacherDto.getTgId(), teacherDto.getTelegramUsername()),
                                                        teacherDto.getId(),
                                                        teacherDto.getEmail(),
                                                        teacherDto.getFirstName(),
                                                        teacherDto.getLastName(),
                                                        teacherDto.getPatronymic()
                                                )) : null
                                ) : null,
                        studentSubjectDto.getLinkToLectorStatement(),
                        studentSubjectDto.getLinkToSubgroupStatement()
                );
            }
            
            else {
                LOGGER.info("Failed to get subject by id {} for {}: code={} body={}", subjectId, user, response.code(), json);
                
                throw toStudentManagerException(json);
            }
        } catch (IOException | RuntimeException e) {
            throw wrapAsUnknownError(e);
        }
    }
    
    
    public StudentSubjectGradesView getStudentGradesBySubjectId(User user, String subjectId) throws StudentManagerException {
        LOGGER.debug("Getting grades by subject id {} for {}", subjectId, user);
        
        Request request = new Request.Builder()
                .method("GET", null)
                .url(BASE_URL + "/student/" + user.getTgUsername() + "/subject/" + subjectId + "/grades")
                .build();
        
        try (Response response = httpClient.newCall(request).execute()) {
            String json = response.body().string();
            
            if (response.isSuccessful()) {
                StudentSubjectGradesDto studentGradesDto = GSON.fromJson(json, StudentSubjectGradesDto.class);
                final List<GradeDto> gradesDto = studentGradesDto.getGrades();
                
                return new StudentSubjectGradesView(
                        studentGradesDto.getSubjectName(),
                        gradesDto != null?
                                gradesDto.stream().map(
                                        gradeDto -> new Grade(
                                                gradeDto.getName(),
                                                gradeDto.getCurrentValue(),
                                                gradeDto.getMaxValue(),
                                                gradeDto.getParts() != null?
                                                        gradeDto.getParts().stream().map(
                                                                gradePartDto -> new GradePart(
                                                                        gradePartDto.getName(),
                                                                        gradePartDto.getValue(),
                                                                        gradePartDto.getMaxValue()
                                                                )
                                                        ).collect(Collectors.toList()) : null
                                        )
                                ).collect(Collectors.toList()) : null,
                        studentGradesDto.getLinkToLecturerStatement(),
                        studentGradesDto.getLinkToSubgroupStatement()
                );
            }
            
            else {
                LOGGER.info("Failed to get grades by subject id {} for {}: code={} body={}",
                        subjectId, user, response.code(), json);
                
                throw toStudentManagerException(json);
            }
        } catch (IOException | RuntimeException e) {
            throw wrapAsUnknownError(e);
        }
    }
}
