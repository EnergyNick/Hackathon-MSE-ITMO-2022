namespace StudentManager.Tables.Models;

public record StudentData : ISheetRowData
{
    public string Id { get; set; }
    public string IsuId { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    public string Telegram { get; set; }
    public string Email { get; set; }
    public string IdGroup { get; set; }
}

internal class StudentsSheet : GoogleSheetFromRowEditor<StudentData>
{
    protected override Dictionary<string, Action<StudentData, object>> InitializeValues { get; }
    
    public StudentsSheet(SheetConnectData sheetConnectData) :
        base(new GoogleSheetEditor(sheetConnectData, "Студенты"))
    {
        
    }
}