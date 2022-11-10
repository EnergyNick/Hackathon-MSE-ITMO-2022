namespace StudentManager.Tables.Models;

public record TeacherData : ISheetRowData
{
    public string Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronymic { get; set; }
    public string Telegram { get; set; }
    public string Email { get; set; }
}