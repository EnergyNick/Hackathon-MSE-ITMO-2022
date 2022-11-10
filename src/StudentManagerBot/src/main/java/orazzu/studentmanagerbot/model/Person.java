package orazzu.studentmanagerbot.model;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;


@NoArgsConstructor
@AllArgsConstructor
@Data
public class Person {
    private User user;
    private String id;
    private String email;
    private String firstName;
    private String lastName;
    private String patronymic;
    
    
    public String getName() {
        StringBuilder sb = new StringBuilder();
        
        if (lastName != null)
            sb.append(lastName);
        
        if (firstName != null) {
            if (!sb.isEmpty())
                sb.append(' ');
            
            sb.append(firstName);
        }
    
        if (patronymic != null) {
            if (!sb.isEmpty())
                sb.append(' ');
        
            sb.append(patronymic);
        }
        
        return sb.toString();
    }
}
