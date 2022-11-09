using Microsoft.Extensions.Configuration;
using StudentManager.Tables.Models;

namespace StudentManager.Tables;

internal record SheetConnectData(string SpreadsheetId, IConfiguration Configuration);

public class ManagerTablesHandler
{
    public const string SpreadsheetId = "1Z4tV3gmqqDrfTH8W-clmsb6SK-e1r0MCcxEI4kiFVVw";

    public ManagerTablesHandler(IConfiguration configuration)
    {
        SheetConnectData connectData = new(SpreadsheetId, configuration);
        
        var statementsSheet = new StatementsSheet(connectData);
        var studentsSheet = new StudentsSheet(connectData);
        
        StudentsStatementInSubgroups sheet = new StudentsStatementInSubgroups(statementsSheet, studentsSheet, connectData);
        var list = sheet.ReadAll().Result;
        foreach (var data in list)
        {
            Console.WriteLine(data);
        }
        
        /*var list = new TeachersSheet(connectData).ReadAll().Result;
        foreach (var data in list)
        {
            Console.WriteLine(data);
        }*/
        //StudentsStatementInSubgroups sheet = new StudentsStatementInSubgroups(connectData, configuration);
        //var list = sheet.ReadAll().Result;
        /*foreach (var data in list)
        {
            Console.WriteLine(data);
        }*/
    }
}