namespace StudentManager.Tables.Models;

internal class TeachersSheet : BaseGoogleSheetFromRowEditor<TeacherData>
{
    protected override Dictionary<string, ColumnCondition<TeacherData>> ColumnsDatas => new()
    {
        ["ID"] =
            new ((data, value) => data.Id = value.ToString(), true),
        ["ФИО"] =
            new ((data, value) =>
            {
                var fcs = value.ToString()!.Split();
                data.Surname = fcs[0];
                data.Name = fcs[1];
                data.Patronymic = fcs.ElementAtOrDefault(2) ?? "";
            }, true),
        ["Telegram"] =
            new ((data, value) => data.Telegram = value.ToString(), false),
        ["Почта"] =
            new ((data, value) => data.Email = value.ToString(), true),
    };

    protected override string LeafSheet => "Преподаватели";
    
    public TeachersSheet(SheetConnectData sheetConnectData)
        : base(sheetConnectData)
    {
    }
}