namespace StartupWing_BlazorServer.Components.Services;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

public class GoogleSheetsService
{
    private static string[] Scopes = [SheetsService.Scope.SpreadsheetsReadonly];
    private static string ApplicationName = "StartUpWingDashboard";
    private static string CredentialsFilePath = $"{Environment.CurrentDirectory}/google-credentials.json";
    private static string SpreadsheetId = "1x9YAeQSLcm4tHL_n7vutENyuVaM2f17jXKNAK1jjzwg";
    
    public async Task<IList<IList<object>>?> Get(string sheetName, string startColumn, string startRow, string endColumn, string endRow)
    {
        string readRange = $"{sheetName}!{startColumn}{startRow}:{endColumn}{endRow}";
        var credential = GoogleCredential.FromFile(CredentialsFilePath).CreateScoped(Scopes);
        
        var sheetsService = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName
        });
        
        var request = sheetsService.Spreadsheets.Values.Get(SpreadsheetId, readRange);
        ValueRange response = await request.ExecuteAsync();
        
        if (response.Values != null && response.Values.Count > 0)
        {
            return response.Values;
        }

        return null;
    }
}