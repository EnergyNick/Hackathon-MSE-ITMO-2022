package orazzu.studentmanagerbot.dao;


import com.google.gson.FieldNamingPolicy;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import orazzu.studentmanagerbot.error.ErrorCode;
import orazzu.studentmanagerbot.error.StudentManagerException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;


public class DaoBase {
    private final Logger LOGGER = LoggerFactory.getLogger(DaoBase.class);
    protected final Gson GSON = new GsonBuilder().setFieldNamingPolicy(FieldNamingPolicy.UPPER_CAMEL_CASE).create();
    
    
    protected StudentManagerException wrapAsUnknownError(Exception e) {
        LOGGER.info("Rethrowing as unknown error", e);
        
        return new StudentManagerException(ErrorCode.UNKNOWN_ERROR);
    }
    
    
    protected StudentManagerException toStudentManagerException(String json) {
        return new StudentManagerException(ErrorCode.valueOf(json));
    }
}
