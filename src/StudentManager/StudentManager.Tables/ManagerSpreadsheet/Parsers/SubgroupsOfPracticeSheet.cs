namespace StudentManager.Tables.Models;

internal class SubgroupsOfPracticeSheet : BaseGoogleSheetFromRowEditor<SubgroupOfPracticeData>
{
    protected override Dictionary<string, ColumnCondition<SubgroupOfPracticeData>> ColumnsDatas { get; } = new()
    {
        ["ID предмета"] =
            new((data, value) => data.IdSubject = value.ToString(), true),
        ["ID преподавателя"] =
            new((data, value) => data.IdTeacher = value.ToString(), true),
        ["Ресурс, куда отправлять ДЗ"] =
            new((data, value) => data.ResourceFromSendingHomerworks = value.ToString(), false),
        ["CSC ссылка"] =
            new((data, value) => data.LinkToCSC = value.ToString(), true),
    };

    protected override int SheetId => 368002650;
    
    public SubgroupsOfPracticeSheet(SheetConnectData sheetConnectData)
        : base(sheetConnectData)
    {
        
    }

    protected override void InitAdditionalyParsedData(SubgroupOfPracticeData data)
    {
        data.Id = $"{data.IdTeacher} - {data.IdSubject}";
    }
}
