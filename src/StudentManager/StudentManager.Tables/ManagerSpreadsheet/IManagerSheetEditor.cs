using StudentManager.Tables.Models;

namespace StudentManager.Tables;

public enum LoadColumnBehaviour
{
    ThrowException,
    Skip,
};

public interface IManagerSheetEditor<T> where T : ISheetRowData
{
    Task<List<T>> ReadAll(LoadColumnBehaviour loadColumnBehaviour = LoadColumnBehaviour.ThrowException);
    Task Update(T value, string id) => Task.CompletedTask;
    
    Task Add(T value, string id) => Task.CompletedTask;
    Task Delete(T value, string id) => Task.CompletedTask;
};