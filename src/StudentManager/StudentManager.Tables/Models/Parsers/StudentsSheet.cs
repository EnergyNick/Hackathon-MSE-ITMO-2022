namespace StudentManager.Tables.Models;

internal class StudentsSheet : BaseGoogleSheetFromRowEditor<StudentData>
{
    protected override Dictionary<string, ColumnCondition<StudentData>> ColumnsDatas { get; } = new()
    {
        ["ID"] =
            new((data, value) => data.Id = value.ToString(), true),
        ["ID ИСУ"] =
            new((data, value) => data.IsuId = value.ToString(), true),
        ["Фамилия"] =
            new((data, value) => data.Surname = value.ToString(), true),
        ["Имя"] =
            new((data, value) => data.Name = value.ToString(), true),
        ["Отчество"] = 
            new ((data, value) => data.Patronymic = value.ToString(), false),
        ["Telegram"] = 
            new ((data, value) => data.Telegram = value.ToString(), true),
        ["Почта"] = 
            new ((data, value) => data.Email = value.ToString(), false),
        ["ID группы"] = 
            new ((data, value) => data.IdGroup = value.ToString(), true),
    };
    
    protected override string LeafSheet => "Студенты";

    public StudentsSheet(SheetConnectData sheetConnectData)
        : base(sheetConnectData)
    {
    }
}