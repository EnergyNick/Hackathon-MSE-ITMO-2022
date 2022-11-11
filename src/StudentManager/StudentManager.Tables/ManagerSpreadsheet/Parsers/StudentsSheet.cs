namespace StudentManager.Tables.Models;

internal class StudentsSheet : BaseGoogleSheetFromRowEditor<StudentData>
{
    protected override Dictionary<string, ColumnCondition<StudentData>> ColumnsDatas { get; } = new()
    {
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
            new ((data, value) => data.Telegram = value.ToString(), false),
        ["Почта"] = 
            new ((data, value) => data.Email = value.ToString(), false),
        ["ID группы"] = 
            new ((data, value) => data.IdGroup = value.ToString(), true),
    };

    protected override int SheetId => 0;

    public StudentsSheet(SheetConnectData sheetConnectData)
        : base(sheetConnectData)
    {
    }
    
    protected override void InitAdditionalyParsedData(StudentData data)
    {
        data.Id = data.IsuId;
    }
}