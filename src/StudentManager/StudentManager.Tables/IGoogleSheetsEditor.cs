namespace StudentManager.Tables;

public interface IGoogleSheetsEditor
{
    Task<List<T>> ReadAll<T>();
    Task Update<T>(T value, string id);
    
    Task Add<T>(T value, string id) => Task.CompletedTask;
    Task Delete<T>(T value, string id) => Task.CompletedTask;
};