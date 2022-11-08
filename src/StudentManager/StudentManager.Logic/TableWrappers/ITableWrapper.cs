using FluentResults;
using StudentManager.Tables.Models;

namespace StudentManager.Logic.TableWrappers;

public interface ITableWrapper<T> where T : ISheetRowData
{
    Task<Result<T>> ReadById(string id);
    Task<List<T>> ReadAll();
    Task<Result> Update(string id, T value);
}