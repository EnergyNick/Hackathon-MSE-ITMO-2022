package orazzu.studentmanagerbot.service.util;


import com.pengrad.telegrambot.request.SendMessage;
import orazzu.studentmanagerbot.model.*;
import orazzu.studentmanagerbot.view.StudentSubjectGradesView;
import orazzu.studentmanagerbot.view.StudentSubjectView;

import java.util.List;


public class EntityToMessageMapper {
    public static SendMessage map(Long userId, StudentSubjectView studentSubject) {
        Subject subject = studentSubject.getSubject();
    
        MessageBuilder msgBuilder = new MessageBuilder(userId)
                .appendHeader(subject.getName());
    
        Lecturer lecturer = subject.getLecturer();
        String cscLink = subject.getCscLink();
        String lecturerStatementLink = studentSubject.getLinkToLecturerStatement();
    
        if (lecturer != null || cscLink != null || lecturerStatementLink != null)
            msgBuilder.appendHeader("Лекции");
    
        if (lecturer != null)
            msgBuilder
                    .appendKeyValue("Лектор", lecturer.getPerson().getName())
                    .appendKeyValueTg("TG", lecturer.getPerson().getUser().getTgUsername())
                    .appendKeyValue("Почта", lecturer.getPerson().getEmail());
    
        msgBuilder.appendKeyValue("Wiki", cscLink);
        msgBuilder.appendKeyValue("Ведомость", lecturerStatementLink);
    
        SubgroupOfSubject subgroup = studentSubject.getSubgroupOfSubject();
        String subgroupStatementLink = studentSubject.getLinkToSubgroupStatement();
    
        if (subgroup != null || subgroupStatementLink != null)
            msgBuilder.appendHeader("Практики");
    
        if (subgroup != null) {
            Teacher teacher = subgroup.getTeacher();
        
            msgBuilder
                    .appendKeyValue("Преподаватель", teacher.getPerson().getName())
                    .appendKeyValueTg("TG", teacher.getPerson().getUser().getTgUsername())
                    .appendKeyValue("Почта", teacher.getPerson().getEmail())
                    .appendKeyValue("Wiki", subgroup.getLinkToCsc());
        }
    
        msgBuilder.appendKeyValue("Ведомость", subgroupStatementLink);
        
        return msgBuilder.build();
    }
    
    
    public static SendMessage map(Long userId, StudentSubjectGradesView studentSubjectGradesView) {
        MessageBuilder msgBuilder = new MessageBuilder(userId)
                .appendHeader(studentSubjectGradesView.getSubjectName());
    
        List<Grade> grades = studentSubjectGradesView.getGrades();
        if (grades != null)
            grades.forEach(grade -> {
                msgBuilder.appendKeyValue(grade.getName(), grade.getCurrentValue() + " из " + grade.getMaxValue());
                
                if (grade.getParts() != null)
                    grade.getParts().forEach(gradePart -> msgBuilder.appendKeyValue(
                            gradePart.getName(),
                            gradePart.getValue() + " из " + gradePart.getMaxValue()
                    ));
            });
        
        msgBuilder.appendHeader("Ведомости");
        msgBuilder
                .appendKeyValue("Лекции", studentSubjectGradesView.getLinkToLecturerStatement())
                .appendKeyValue("Практики", studentSubjectGradesView.getLinkToSubgroupStatement());
        
        return msgBuilder.build();
    }
}
