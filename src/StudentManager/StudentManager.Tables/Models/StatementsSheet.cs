namespace StudentManager.Tables.Models;

public enum SessionType
{
    Lecture,
    Practice,
};

public record StatementSheetData(string Id, string IdItem, SessionType SessionType, string IdSubgroup,
    string IdSubject, string IdLeafSheet, string StudentStartCell, string PointsStartCell);

internal class StatementsSheet
{
    
}
