using Microsoft.Extensions.Configuration;
using StudentManager.Tables.Models;

namespace StudentManager.Tables;

internal record SheetConnectData(string SpreadsheetId, IConfiguration Configuration);

public class ManagerTablesHandler
{
    private const string _spreadsheetId = "1Z4tV3gmqqDrfTH8W-clmsb6SK-e1r0MCcxEI4kiFVVw";

    private readonly StudentsTable _table;

    public ManagerTablesHandler(IConfiguration configuration)
    {
        SheetConnectData connectData = new(_spreadsheetId, configuration);
        _table = new StudentsTable(connectData);
    }

    public void GetStudentsTable()
    {
        _table.Load();
    }
}