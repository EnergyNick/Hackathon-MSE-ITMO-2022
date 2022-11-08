namespace StudentManager.Tables.Models;

public record AcademicSubjectData : ISheetRowData
{
    public string Id { get; set; }
    public string IdTeacher { get; set; }
    public string IdGroup { get; set; }
    public string Title { get; set; }
    public string LinkToCSC { get; set; }
    public string Term { get; set; }
}

internal class AcademicSubjectSheet : BaseGoogleSheetFromRowEditor<AcademicSubjectData>
{
    protected override Dictionary<string, Action<AcademicSubjectData, object>> InitializeValues => new()
    {
        ["ID"] = (data, value) => data.Id = value.ToString(),
        ["ID лектора"] = (data, value) => data.IdTeacher = value.ToString(),
        ["ID группы"] = (data, value) => data.IdGroup = value.ToString(),
        ["Название"] = (data, value) => data.Title = value.ToString(),
        ["CSC ссылка"] = (data, value) => data.LinkToCSC = value.ToString(),
        ["Семестр"] = (data, value) => data.Term = value.ToString(),
    };

    protected override string LeafSheet => "Предметы";

    public AcademicSubjectSheet(SheetConnectData sheetConnectData) : base(sheetConnectData)
    {
    }
}
