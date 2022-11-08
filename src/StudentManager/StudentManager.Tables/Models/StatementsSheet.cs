namespace StudentManager.Tables.Models;

public enum StatementType
{
    Practice,
    Lecture,
};

public class StatementSheetData : ISheetRowData
{
    public string Id { get; set; }
    public string IdSubject { get; set; }
    public StatementType StatementType { get; set; }
    public string IdSubgroup { get; set; }
    public string IdSheet { get; set; }
    public string IdLeafSheet { get; set; }
    public string StudentsStartCell { get; set; }
    public string PointsStartCell { get; set; }
}

internal class StatementsSheet : BaseGoogleSheetFromRowEditor<StatementSheetData>
{
    protected override Dictionary<string, Action<StatementSheetData, object>> InitializeValues => new()
    {
        ["ID"] = (data, value) => data.Id = value.ToString(),
        ["ID предмета"] = (data, value) => data.IdSubject = value.ToString(),
        ["Тип занятия"] = (data, value) => data.StatementType = (StatementType)Convert.ToInt32(value),
        ["ID подгруппы"] = (data, value) => data.IdSubgroup = value.ToString(),
        ["ID таблицы ведомости"] = (data, value) => data.IdSheet = value.ToString(),
        ["Имя листа"] = (data, value) => data.IdLeafSheet = value.ToString(),
        ["Ячейка начала студентов"] = (data, value) => data.StudentsStartCell = value.ToString(),
        ["Ячейка начала баллов"] = (data, value) => data.PointsStartCell = value.ToString(),
    };
    
    protected override string LeafSheet { get; }
    
    public StatementsSheet(SheetConnectData sheetConnectData)
        : base(sheetConnectData)
    {
    }
}
