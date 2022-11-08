namespace StudentManager.Tables;

public interface IGoogleSheet<T>
{
    Task<List<T>> ReadAll();
    Task Update(T value, string id) => Task.CompletedTask;
    
    Task Add(T value, string id) => Task.CompletedTask;
    Task Delete(T value, string id) => Task.CompletedTask;
};