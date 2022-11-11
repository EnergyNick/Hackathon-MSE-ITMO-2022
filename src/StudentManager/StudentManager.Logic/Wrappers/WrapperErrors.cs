namespace StudentManager.Logic.Wrappers;

public static class WrapperErrors
{
    // ERRORS
    public static readonly string EmptyInGoogleTablesCache = "ERROR_ON_PARSING_TABLES";
    public static readonly string ErrorOnUploadingGradesToTable = "ERROR_ON_UPLOADING_GRADES_TO_TABLE";

    // Invalid search
    public static readonly string CantFindItemById = "ELEMENT_NOT_FOUND_BY_ID";
    public static readonly string CantFindUserByTelegramUsername = "USER_NOT_FOUND_BY_TELEGRAM_USERNAME";
    public static readonly string CantFindGradesByUserId = "CANT_FIND_GRADES_BY_USER_ID";
}