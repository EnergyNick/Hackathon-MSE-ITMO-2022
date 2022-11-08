using Microsoft.Extensions.Configuration;
using StudentManager.Tables.Models;

namespace StudentManager.Tables;

internal record SheetConnectData(string SpreadsheetId, IConfiguration Configuration);

public class ManagerTablesHandler
{
    private const string _spreadsheetId = "1Z4tV3gmqqDrfTH8W-clmsb6SK-e1r0MCcxEI4kiFVVw";

    public ManagerTablesHandler(IConfiguration configuration)
    {
        // SheetConnectData connectData = new(_spreadsheetId, configuration);
        // var list = new StudentsSheet(connectData).ReadAll().Result;
        // foreach (var data in list)
        // {
        //     Console.WriteLine(data);
        // }
    }
}