namespace StudentManager.Tables.Models;

internal class StudentsSheet : BaseGoogleSheetFromRowEditor<StudentData>
{
    protected override Dictionary<string, ColumnCondition<StudentData>> ColumnsDatas { get; } = new()
    {
        ["ID"] =
            new((data, value) => data.Id = value.ToString()!, true),
        ["ID ИСУ"] =
            new((data, value) => data.IsuId = value.ToString()!, true),
        ["ФИО"] =
            new ((data, value) =>
            {
                var fcs = value.ToString()!.Split();
                data.Surname = fcs[0];
                data.Name = fcs[1];
                data.Patronymic = fcs.ElementAtOrDefault(2) ?? "";
            }, true),
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