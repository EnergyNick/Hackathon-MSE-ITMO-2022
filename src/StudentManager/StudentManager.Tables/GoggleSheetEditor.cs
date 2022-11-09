using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;

namespace StudentManager.Tables;

internal class GoogleSheetEditor
{
    private const string _keyFromGetGoogleAPIToken = "GoogleAPIToken";
    
    private readonly string _spreadsheetId;
    private readonly string _sheetNameAndRange;
    private SheetsService _service;

    public GoogleSheetEditor(SheetConnectData sheetConnectData, int sheetId)
    {
        _spreadsheetId = sheetConnectData.SpreadsheetId;
        InitService(sheetConnectData);
        _sheetNameAndRange = GetSheetNameByID(sheetId).Result;
    }

    public GoogleSheetEditor(SheetConnectData sheetConnectData, string sheetNameAndRange)
    {
        _spreadsheetId = sheetConnectData.SpreadsheetId;
        InitService(sheetConnectData);
        _sheetNameAndRange = sheetNameAndRange;
    }
    
    public GoogleSheetEditor(SheetConnectData sheetConnectData, int sheetId, string sheetRange)
    {
        _spreadsheetId = sheetConnectData.SpreadsheetId;
        InitService(sheetConnectData);
        _sheetNameAndRange = $"'{GetSheetNameByID(sheetId).Result}'!{sheetRange}";
    }

    public async Task<IList<IList<object>>> GetSheet()
    {
        SpreadsheetsResource.ValuesResource.GetRequest request = _service.Spreadsheets.Values.Get(_spreadsheetId, _sheetNameAndRange);

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

    public void SetSheet(IList<IList<object>> sheet)
    {
        ValueRange body = new ValueRange()
        {
            Values = sheet,
        };

        var updateRequest = _service.Spreadsheets.Values.Update(body, _spreadsheetId, _sheetNameAndRange);
        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        updateRequest.Execute();
    }

    private void InitService(SheetConnectData sheetConnectData)
    {
        string jsonToken = sheetConnectData.Configuration.GetSection(_keyFromGetGoogleAPIToken).Value;
        var credential = GoogleCredential.FromJson(jsonToken).UnderlyingCredential as ServiceAccountCredential;
        
        _service = new SheetsService(
            new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            }
        );
    }
    
    public async Task<string> GetSheetNameByID(int sheetID)
    {
        var ranges = Array.Empty<string>();
        bool includeGridData = false;

        SpreadsheetsResource.GetRequest request = _service.Spreadsheets.Get(_spreadsheetId);
        request.Ranges = ranges;
        request.IncludeGridData = includeGridData;

        var response = await request.ExecuteAsync();

        foreach(var sheet in response.Sheets)
        {
            if (sheet.Properties.SheetId == sheetID)
            {
                return sheet.Properties.Title;
            }
        }

        throw new ArgumentNullException("No SheetTitle was found for this SheetId.");
    }
}