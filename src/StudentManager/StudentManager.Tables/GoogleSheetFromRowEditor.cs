using System.Collections;
using System.Collections.Generic;

namespace StudentManager.Tables;

internal abstract class GoogleSheetFromRowEditor<T> : IGoogleSheet<T> where T : new ()
{
    protected record AvailableColumn(string Value, int Index);

    protected abstract Dictionary<string, Action<T, object>> InitializeValues { get; }

    private readonly GoogleSheetEditor _googleSheetFromRowEditor;

    protected GoogleSheetFromRowEditor(GoogleSheetEditor googleSheetEditor)
    {
        _googleSheetFromRowEditor = googleSheetEditor;
    }

    public async Task<List<T>> ReadAll()
    {
        IList<IList<object>> sheetValues = await _googleSheetFromRowEditor.GetSheet();

        var availableColumns = CheckColumnsFromSpec(sheetValues);
        var values = Parse(sheetValues, availableColumns);
        return values;
    }

    private List<AvailableColumn> CheckColumnsFromSpec(IList<IList<object>> sheetValues)
    {
        List<AvailableColumn> availableColumns = new List<AvailableColumn>();
        var namesColumn = sheetValues[0];
        for (int i = 0; i < namesColumn.Count; ++i)
        {
            var nameColumn = namesColumn[i].ToString();
            /*if (ContainsColumn(nameColumn))
                availableColumns.Add(new AvailableColumn(nameColumn, i));*/
        }

        return availableColumns;
    }

    protected List<T> Parse(IList<IList<object>> sheetValues, IList<AvailableColumn> availableColumns)
    {
        var datas = new List<T>();
        for (int i = 1; i < sheetValues.Count; i++)
        {
            var data = new T();
            foreach (var column in availableColumns)
            {
                InitializeValues[column.Value].Invoke(data, sheetValues[i][column.Index]);
            }
            datas.Add(data);
        }

        return datas;
    }
}