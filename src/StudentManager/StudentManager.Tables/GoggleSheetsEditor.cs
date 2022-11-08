using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;

namespace StudentManager.Tables;

internal class GoogleSheetFromRowEditor
{
    private const string _keyFromGetGoogleAPIToken = "GoogleAPIToken";
    
    private readonly SheetsService _service;
    private readonly string _spreadsheetId;
    private readonly string _sheetNameAndRange;

    public GoogleSheetFromRowEditor(SheetConnectData sheetConnectData, string sheetNameAndRange)
    {
        _spreadsheetId = sheetConnectData.SpreadsheetId;
        _sheetNameAndRange = sheetNameAndRange;
        
        string jsonToken = sheetConnectData.Configuration.GetSection(_keyFromGetGoogleAPIToken).Value;
        var credential = GoogleCredential.FromJson(jsonToken).UnderlyingCredential as ServiceAccountCredential;
        
        _service = new SheetsService(
            new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            }
        );
    }

    public IList<IList<object>> GetSheet()
    {
        SpreadsheetsResource.ValuesResource.GetRequest request = _service.Spreadsheets.Values.Get(_spreadsheetId, _sheetNameAndRange);

        ValueRange response = request.Execute();
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
}