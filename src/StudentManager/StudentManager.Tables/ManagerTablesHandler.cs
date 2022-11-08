using Microsoft.Extensions.Configuration;
using StudentManager.Tables.Models;

namespace StudentManager.Tables;

internal record SheetConnectData(string SpreadsheetId, IConfiguration Configuration);

public class ManagerTablesHandler
{
    private const string _spreadsheetId = "1Z4tV3gmqqDrfTH8W-clmsb6SK-e1r0MCcxEI4kiFVVw";

    private readonly StudentsSheet _sheet;

    public ManagerTablesHandler(IConfiguration configuration)
    {
        SheetConnectData connectData = new(_spreadsheetId, configuration);
        _sheet = new StudentsSheet(connectData);
    }

    public Task<List<T>> ReadAll<T>()
    {
        throw new NotImplementedException();
    }

    public Task Update<T>(T value, string id)
    {
        throw new NotImplementedException();
    }
}