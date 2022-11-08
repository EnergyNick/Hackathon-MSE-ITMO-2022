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

internal class StudentsSheet : BaseGoogleSheetFromRowEditor<StudentData>
{
    protected override Dictionary<string, Action<StudentData, object>> InitializeValues => new()
    {
        ["ID"] = (data, value) => data.Id = value.ToString(),
        ["ID ИСУ"] = (data, value) => data.IsuId = value.ToString(),
        ["Фамилия"] = (data, value) => data.Surname = value.ToString(),
        ["Имя"] = (data, value) => data.Name = value.ToString(),
        ["Отчество"] = (data, value) => data.Patronymic = value.ToString(),
        ["Telegram"] = (data, value) => data.Telegram = value.ToString(),
        ["Почта"] = (data, value) => data.Email = value.ToString(),
        ["ID группы"] = (data, value) => data.IdGroup = value.ToString(),
    };
    
    protected override string LeafSheet => "Студенты";

    public StudentsSheet(SheetConnectData sheetConnectData)
        : base(sheetConnectData)
    {
    }
}