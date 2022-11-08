namespace StudentManager.Tables.Models;

public class SubgroupOfPracticeData : ISheetRowData
{
    public string Id { get; set; }
    public string IdSubject { get; set; }
    public string IdTeacher { get; set; }
    public string LinkToCSC { get; set; }
    public string LinkToStatement { get; set; }
}

internal class SubgroupsOfPracticeSheet
{
    
}
