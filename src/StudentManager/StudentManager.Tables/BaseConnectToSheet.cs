using System.Collections;
using System.Collections.Generic;

namespace StudentManager.Tables;

internal abstract class BaseConnectToSheet<T>
{
    protected record AvailableColumn(string Value, int Index);

    private readonly GoogleSheetsEditor _googleSheetsEditor;

    protected BaseConnectToSheet(GoogleSheetsEditor googleSheetsEditor)
    {
        _googleSheetsEditor = googleSheetsEditor;
    }

    public List<T> Load()
    {
        IList<IList<object>> sheetValues = _googleSheetsEditor.GetSheet();

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
}
