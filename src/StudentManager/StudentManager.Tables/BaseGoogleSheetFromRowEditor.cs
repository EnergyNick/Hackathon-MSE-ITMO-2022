using StudentManager.Tables.Models;

namespace StudentManager.Tables;

public record ColumnCondition<T>(Action<T, object> FillValue, bool IsRequired);

internal abstract class BaseGoogleSheetFromRowEditor<T> : IManagerSheetEditor<T> where T : ISheetRowData, new ()
{
    protected record AvailableColumn(string Value, int Index);

    protected abstract Dictionary<string, ColumnCondition<T>> ColumnsDatas { get; }
    protected abstract string LeafSheet { get; }

    private readonly GoogleSheetEditor _googleSheetFromRowEditor;

    protected BaseGoogleSheetFromRowEditor(SheetConnectData sheetConnectData)
    {
        _googleSheetFromRowEditor = new GoogleSheetEditor(sheetConnectData, LeafSheet);
    }

    public async Task<List<T>> ReadAll(LoadColumnBehaviour loadColumnBehaviour = LoadColumnBehaviour.ThrowException)
    {
        IList<IList<object>> sheetValues = await _googleSheetFromRowEditor.GetSheet();

        var availableColumns = CheckColumnsFromSpec(sheetValues, loadColumnBehaviour);
        var values = Parse(sheetValues, availableColumns, loadColumnBehaviour);
        return values;
    }

    private List<AvailableColumn> CheckColumnsFromSpec(IList<IList<object>> sheetValues,
        LoadColumnBehaviour loadColumnBehaviour)
    {
        List<AvailableColumn> availableColumns = new List<AvailableColumn>();
        var namesColumn = sheetValues[0];
        var requiredColumnsClone = ColumnsDatas.Where((pair) => pair.Value.IsRequired).Select((pair) => pair.Key).ToHashSet();
        for (int i = 0; i < namesColumn.Count; ++i)
        {
            var nameColumn = namesColumn[i].ToString();
            if (requiredColumnsClone.Contains(nameColumn))
            {
                availableColumns.Add(new AvailableColumn(nameColumn, i));
                requiredColumnsClone.Remove(nameColumn);
            }
        }

        if (requiredColumnsClone.Count != 0)
        {
            string errorOutput = "All required columns were not found. These columns could not be found:";
            foreach (var requiredColumn in requiredColumnsClone)
            {
                errorOutput += $"\n{requiredColumn}";
            }

            throw new InvalidOperationException(errorOutput);
        }

        return availableColumns;
    }

    protected List<T> Parse(IList<IList<object>> sheetValues, IList<AvailableColumn> availableColumns,
        LoadColumnBehaviour loadColumnBehaviour)
    {
        var datas = new List<T>();
        for (int i = 1; i < sheetValues.Count; i++)
        {
            var data = new T();
            foreach (var column in availableColumns)
            {
                var columnData = ColumnsDatas[column.Value];
                if (loadColumnBehaviour == LoadColumnBehaviour.ThrowException &&
                    columnData.IsRequired && sheetValues[i][column.Index].ToString() == "")
                    throw new InvalidOperationException(
                        $"The value in the required column is empty:\nColumn Name: {column.Value}\nIndex Row: {i}");
                
                ColumnsDatas[column.Value].FillValue.Invoke(data, sheetValues[i][column.Index]);
            }
            datas.Add(data);
        }

        return datas;
    }
}