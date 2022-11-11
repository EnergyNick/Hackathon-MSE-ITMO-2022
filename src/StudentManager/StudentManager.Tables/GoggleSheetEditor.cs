using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using MajorDimensionEnum = Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum;
using BatchMajorDimensionEnum = Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.BatchGetRequest.MajorDimensionEnum;

namespace StudentManager.Tables;

internal class GoogleSheetEditor
{
    private const string _keyFromGetGoogleAPIToken = "GoogleAPIToken";
    
    public readonly string SpreadsheetId;
    private readonly string _sheetNameAndRange;
    private SheetsService _service;

    public GoogleSheetEditor(SheetConnectData sheetConnectData, int sheetId)
    {
        SpreadsheetId = sheetConnectData.SpreadsheetId;
        _service = GetSheetsService(sheetConnectData);
        _sheetNameAndRange = GetSheetNameByID(_service, sheetConnectData.SpreadsheetId, sheetId).Result;
    }

    public GoogleSheetEditor(SheetConnectData sheetConnectData, string sheetNameAndRange = "")
    {
        SpreadsheetId = sheetConnectData.SpreadsheetId;
        _service = GetSheetsService(sheetConnectData);
        _sheetNameAndRange = sheetNameAndRange;
    }
    
    public GoogleSheetEditor(SheetConnectData sheetConnectData, int sheetId, string sheetRange)
    {
        SpreadsheetId = sheetConnectData.SpreadsheetId;
        _service = GetSheetsService(sheetConnectData);
        _sheetNameAndRange = $"'{GetSheetNameByID(_service, sheetConnectData.SpreadsheetId, sheetId).Result}'!{sheetRange}";
    }

    public GoogleSheetEditor(SheetConnectData sheetConnectData, SheetsService sheetsService, string sheetNameAndRange = "")
    {
        SpreadsheetId = sheetConnectData.SpreadsheetId;
        _service = sheetsService;
        _sheetNameAndRange = sheetNameAndRange;
    }

    public async Task<IList<IList<object>>> GetSheet(MajorDimensionEnum majorDimension = MajorDimensionEnum.ROWS)
    {
        SpreadsheetsResource.ValuesResource.GetRequest request =
            _service.Spreadsheets.Values.Get(SpreadsheetId, _sheetNameAndRange);
        request.MajorDimension = majorDimension;

        ValueRange response = await request.ExecuteAsync();
        IList<IList<object>> sheet = response.Values;
        if (sheet != null && sheet.Count > 0)
        {
            return sheet;
        }
        else
        {
            throw new InvalidOperationException("No data found.");
        }
    }
    
    public async Task<IList<ValueRange>> GetBatchSheet(List<string> ranges, BatchMajorDimensionEnum majorDimension = BatchMajorDimensionEnum.ROWS)
    {
        SpreadsheetsResource.ValuesResource.BatchGetRequest request =
            _service.Spreadsheets.Values.BatchGet(SpreadsheetId);
        request.MajorDimension = majorDimension;
        request.Ranges = ranges;

        BatchGetValuesResponse response = await request.ExecuteAsync();
        return response.ValueRanges;
    }

    public async Task SetSheet(IList<IList<object>> sheet)
    {
        ValueRange body = new ValueRange()
        {
            Values = sheet,
        };
        
        var updateRequest = _service.Spreadsheets.Values.Update(body, SpreadsheetId, _sheetNameAndRange);
        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        await updateRequest.ExecuteAsync();
    }
    
    public async Task SetSheet(IList<Request> requests)
    {
        var body = new BatchUpdateSpreadsheetRequest()
        {
            Requests = requests,
        };

        var updateRequest = _service.Spreadsheets.BatchUpdate(body, SpreadsheetId);
        //updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        await updateRequest.ExecuteAsync();
    }

    public static string GetSpreadsheetIdFromLink(string link)
    {
        return link.Split("https://docs.google.com/spreadsheets/d/")[1].Split("/edit")[0];
    }

    public static SheetsService GetSheetsService(SheetConnectData sheetConnectData)
    {
        string jsonToken = sheetConnectData.Configuration.GetSection(_keyFromGetGoogleAPIToken).Value;
        var credential = GoogleCredential.FromJson(jsonToken).UnderlyingCredential as ServiceAccountCredential;
        
        return new SheetsService(
            new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            }
        );
    }
    
    public static async Task<string> GetSheetNameByID(SheetsService service, string spreadsheetId, int sheetID)
    {
        var response = await GetSpreadsheetResponseInfo(service, spreadsheetId);

        foreach(var sheet in response.Sheets)
            if (sheet.Properties.SheetId == sheetID)
                return sheet.Properties.Title;

        throw new ArgumentNullException("No SheetTitle was found for this SheetId.");
    }

    public static async Task<int> GetIdBySheetName(SheetsService service, string spreadsheetId, string sheetName)
    {
        var response = await GetSpreadsheetResponseInfo(service, spreadsheetId);

        foreach(var sheet in response.Sheets)
            if (sheet.Properties.Title == sheetName)
                return sheet.Properties.SheetId.Value;

        throw new ArgumentNullException("No SheetId was found for this SheetName.");
    }

    private static async Task<Spreadsheet> GetSpreadsheetResponseInfo(SheetsService service, string spreadsheetId)
    {
        var ranges = Array.Empty<string>();
        bool includeGridData = false;

        SpreadsheetsResource.GetRequest request = service.Spreadsheets.Get(spreadsheetId);
        request.Ranges = ranges;
        request.IncludeGridData = includeGridData;

        var response = await request.ExecuteAsync();
        return response;
    }
}