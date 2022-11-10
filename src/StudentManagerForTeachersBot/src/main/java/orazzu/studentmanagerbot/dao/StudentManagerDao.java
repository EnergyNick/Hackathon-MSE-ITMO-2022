package orazzu.studentmanagerbot.dao;


import okhttp3.OkHttpClient;
import orazzu.studentmanagerbot.Props;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;


public class StudentManagerDao extends DaoBase {
    private final Logger LOGGER = LoggerFactory.getLogger(StudentManagerDao.class);
    private final OkHttpClient httpClient = new OkHttpClient();
    private final String BASE_URL = Props.getProp("student_manager_api.url");
}
