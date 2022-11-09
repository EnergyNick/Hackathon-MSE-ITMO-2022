namespace StudentManager.Tables.Models;

internal class TeachersSheet : BaseGoogleSheetFromRowEditor<TeacherData>
{
    protected override Dictionary<string, ColumnCondition<TeacherData>> ColumnsDatas => new()
    {
        ["ID"] =
            new ((data, value) => data.Id = value.ToString(), true),
        ["Фамилия"] =
            new ((data, value) => data.Surname = value.ToString(), true),
        ["Имя"] =
            new ((data, value) => data.Name = value.ToString(), true),
        ["Отчество"] =
            new ((data, value) => data.Patronymic = value.ToString(), false),
        ["Telegram"] =
            new ((data, value) => data.Telegram = value.ToString(), true),
        ["Почта"] =
            new ((data, value) => data.Email = value.ToString(), true),
    };

    protected override string LeafSheet => "Преподаватели";
    
    public TeachersSheet(SheetConnectData sheetConnectData)
        : base(sheetConnectData)
    {
    }
}