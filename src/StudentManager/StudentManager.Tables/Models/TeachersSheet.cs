namespace StudentManager.Tables.Models;

public class TeacherData : ISheetRowData
{
    public string Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    public string Telegram { get; set; }
    public string Email { get; set; }
}

internal class TeachersSheet : BaseGoogleSheetFromRowEditor<TeacherData>
{
    protected override Dictionary<string, Action<TeacherData, object>> InitializeValues => new()
    {
        ["ID"] = (data, value) => data.Id = value.ToString(),
        ["Фамилия"] = (data, value) => data.Surname = value.ToString(),
        ["Имя"] = (data, value) => data.Name = value.ToString(),
        ["Отчество"] = (data, value) => data.Patronymic = value.ToString(),
        ["Telegram"] = (data, value) => data.Telegram = value.ToString(),
        ["Почта"] = (data, value) => data.Email = value.ToString(),
    };

    protected override string LeafSheet => "Преподаватели";
    
    public TeachersSheet(SheetConnectData sheetConnectData)
        : base(sheetConnectData)
    {
    }
}