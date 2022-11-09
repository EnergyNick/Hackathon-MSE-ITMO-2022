namespace StudentManager.Tables.Models;

internal class SubgroupsOfPracticeSheet : BaseGoogleSheetFromRowEditor<SubgroupOfPracticeData>
{
    protected override Dictionary<string, ColumnCondition<SubgroupOfPracticeData>> ColumnsDatas { get; } = new()
    {
        ["ID"] =
            new((data, value) => data.Id = value.ToString(), true),
        ["ID предмета"] =
            new((data, value) => data.IdSubject = value.ToString(), true),
        ["ID преподавателя"] =
            new((data, value) => data.IdTeacher = value.ToString(), true),
        ["CSC ссылка"] =
            new((data, value) => data.LinkToCSC = value.ToString(), true),
    };

    protected override int SheetId => 368002650;
    
    public SubgroupsOfPracticeSheet(SheetConnectData sheetConnectData)
        : base(sheetConnectData)
    {
        
    }
}
