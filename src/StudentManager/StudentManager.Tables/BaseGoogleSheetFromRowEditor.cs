using StudentManager.Tables.Models;

namespace StudentManager.Tables;

public record ColumnCondition<T>(Action<T, object> FillValue, bool IsRequired);

internal abstract class BaseGoogleSheetFromRowEditor<T> : IManagerSheetEditor<T> where T : ISheetRowData, new ()
{
    protected record AvailableColumn(string Value, int Index);

    protected abstract Dictionary<string, ColumnCondition<T>> ColumnsDatas { get; }
    protected abstract int SheetId { get; }

    private readonly GoogleSheetEditor _googleSheetFromRowEditor;

    protected BaseGoogleSheetFromRowEditor(SheetConnectData sheetConnectData)
    {
        _googleSheetFromRowEditor = new GoogleSheetEditor(sheetConnectData, SheetId);
    }

    public async Task<List<T>> ReadAll(LoadColumnBehaviour loadColumnBehaviour = LoadColumnBehaviour.ThrowException)
    {
        IList<IList<object>> sheetValues = await _googleSheetFromRowEditor.GetSheet();

        var availableColumns = CheckColumnsFromSpec(sheetValues, loadColumnBehaviour);
        var values = Parse(sheetValues, availableColumns, loadColumnBehaviour);
        return values;
    }

    protected virtual void InitAdditionalyParsedData(T data) { }

    private List<AvailableColumn> CheckColumnsFromSpec(IList<IList<object>> sheetValues,
        LoadColumnBehaviour loadColumnBehaviour)
    {
        List<AvailableColumn> availableColumns = new List<AvailableColumn>();
        var namesColumn = sheetValues[0];
        var requiredColumnsClone = ColumnsDatas.Where((pair) => pair.Value.IsRequired).Select((pair) => pair.Key).ToHashSet();
        for (int i = 0; i < namesColumn.Count; ++i)
        {
            var nameColumn = namesColumn[i].ToString();
            if (ColumnsDatas.ContainsKey(nameColumn))
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

    private List<T> Parse(IList<IList<object>> sheetValues, IList<AvailableColumn> availableColumns,
        LoadColumnBehaviour loadColumnBehaviour)
    {
        var datas = new List<T>();
        for (int i = 1; i < sheetValues.Count; i++)
        {
            var data = new T();
            for (var j = 0; j < availableColumns.Count && j < sheetValues[i].Count; j++)
            {
                var column = availableColumns[j];
                var columnData = ColumnsDatas[column.Value];
                string valueStr = sheetValues[i][column.Index].ToString();
                if (loadColumnBehaviour == LoadColumnBehaviour.ThrowException &&
                    columnData.IsRequired && valueStr is "" or "-")
                    throw new InvalidOperationException(
                        $"The value in the required column is empty:\nColumn Name: {column.Value}\nIndex Row: {i}");

                columnData.FillValue.Invoke(data, sheetValues[i][column.Index]);
            }

            InitAdditionalyParsedData(data);
            datas.Add(data);
        }

        return datas;
    }
}