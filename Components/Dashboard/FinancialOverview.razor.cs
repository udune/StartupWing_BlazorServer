using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Dashboard;

public partial class FinancialOverview : ComponentBase
{
    private bool on;
    private string title;
    private string unit;
    private IList<IList<object>> FinancialOverview_Data;
    
    [Parameter] 
    public string Company { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        await RefreshData();
        on = true;
    }
    
    private async Task RefreshData()
    {
        var sheetData = await GoogleSheetsService.Get(Company, "A", "38", "E", "47");
        
        if (sheetData != null)
        {
            title = (string) sheetData[0][0];
            unit = (string)sheetData[0][4];
            FinancialOverview_Data = sheetData;
        }
    }
}