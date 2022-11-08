using System.Collections;
using System.Collections.Generic;

namespace StudentManager.Tables;

internal abstract class BaseConnectToSheet<T> : IGoogleSheet<T>
{
    protected record AvailableColumn(string Value, int Index);

    private readonly GoogleSheetFromRowEditor _googleSheetFromRowEditor;

    protected BaseConnectToSheet(GoogleSheetFromRowEditor googleSheetFromRowEditor)
    {
        _googleSheetFromRowEditor = googleSheetFromRowEditor;
    }

    public List<T> Load()
    {
        IList<IList<object>> sheetValues = _googleSheetFromRowEditor.GetSheet();

        List<AvailableColumn> avaliableColumns = new List<AvailableColumn>();
        var namesColumn = sheetValues[0];
        for (int i = 0; i < namesColumn.Count; ++i)
        {
            var nameColumn = namesColumn[i].ToString();
            if (ContainsColumn(nameColumn))
                avaliableColumns.Add(new AvailableColumn(nameColumn, i));
        }

        return Parse(sheetValues, avaliableColumns);
    }

    protected abstract List<T> Parse(IList<IList<object>> sheetValues, IList<AvailableColumn> availableColumns);

    protected abstract bool ContainsColumn(string nameColumn);
    public Task<List<T>> ReadAll()
    {
        throw new NotImplementedException();
    }

    public Task Update(T value, string id)
    {
        throw new NotImplementedException();
    }
}
