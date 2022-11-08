namespace StudentManager.Tables.Models;

public class SubgroupOfPracticeData : ISheetRowData
{
    public string Id { get; set; }
    public string IdSubject { get; set; }
    public string IdTeacher { get; set; }
    public string LinkToCSC { get; set; }
}

internal class SubgroupsOfPracticeSheet : BaseGoogleSheetFromRowEditor<SubgroupOfPracticeData>
{
    protected override Dictionary<string, Action<SubgroupOfPracticeData, object>> InitializeValues => new()
    {
        ["ID"] = (data, value) => data.Id = value.ToString(),
        ["ID предмета"] = (data, value) => data.IdSubject = value.ToString(),
        ["ID преподавателя"] = (data, value) => data.IdTeacher = value.ToString(),
        ["CSC ссылка"] = (data, value) => data.LinkToCSC = value.ToString(),
    };

    protected override string LeafSheet => "Подгруппы практик";
    
    public SubgroupsOfPracticeSheet(SheetConnectData sheetConnectData)
        : base(sheetConnectData)
    {
        
    }
}
