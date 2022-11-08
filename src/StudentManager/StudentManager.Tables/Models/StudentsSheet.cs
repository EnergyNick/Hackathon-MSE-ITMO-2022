namespace StudentManager.Tables.Models;

public record StudentData(string Id, string IsuId, string Surname, string Name, string Patronymic,
    string Telegram, string Email, string IdGroup);

internal class StudentsSheet : BaseConnectToSheet<StudentData>
{
    public StudentsSheet(SheetConnectData sheetConnectData) :
        base(new GoogleSheetFromRowEditor(sheetConnectData, "Студенты"))
    {
        
    }

    protected override List<StudentData> Parse(IList<IList<object>> sheetValues, IList<AvailableColumn> availableColumns)
    {
        foreach (var column in availableColumns)
        {
            Console.WriteLine(column.Value);
        }

        return null;
    }

    protected override bool ContainsColumn(string nameColumn)
    {
        return true;
    }
}