namespace StudentManager.Tables.Models;

internal class AcademicSubjectsSheet : BaseGoogleSheetFromRowEditor<AcademicSubjectData>
{
    protected override Dictionary<string, ColumnCondition<AcademicSubjectData>> ColumnsDatas { get; } = new()
    {
        ["ID"] =
            new((data, value) => data.Id = value.ToString(), true),
        ["ID лектора"] =
            new((data, value) => data.IdTeacher = value.ToString(), true),
        ["ID группы"] =
            new((data, value) => data.IdGroup = value.ToString(), true),
        ["Название"] =
            new((data, value) => data.Title = value.ToString(), true),
        ["CSC ссылка"] =
            new((data, value) => data.LinkToCSC = value.ToString(), true),
        ["Семестр"] =
            new((data, value) => data.Term = value.ToString(), true),
    };

    protected override int SheetId => 1936532220;

    public AcademicSubjectsSheet(SheetConnectData sheetConnectData) : base(sheetConnectData)
    {
    }
}
