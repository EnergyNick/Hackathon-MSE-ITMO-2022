package orazzu.studentmanagerbot.service.util;


import com.pengrad.telegrambot.request.SendMessage;
import orazzu.studentmanagerbot.model.Lecturer;
import orazzu.studentmanagerbot.model.SubgroupOfSubject;
import orazzu.studentmanagerbot.model.Subject;
import orazzu.studentmanagerbot.model.Teacher;
import orazzu.studentmanagerbot.view.StudentSubjectView;


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
                    .appendKeyValue("TG", lecturer.getPerson().getUser().getTgUsername())
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
                    .appendKeyValue("TG", teacher.getPerson().getUser().getTgUsername())
                    .appendKeyValue("Почта", teacher.getPerson().getEmail())
                    .appendKeyValue("Wiki", subgroup.getLinkToCsc());
        }
    
        msgBuilder.appendKeyValue("Ведомость", subgroupStatementLink);
        
        return msgBuilder.build();
    }
}
