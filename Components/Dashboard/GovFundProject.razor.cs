using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Dashboard;

public partial class GovFundProject : ComponentBase
{
    private bool on;
    private string title;
    private string unit;
    private IList<IList<object>> GovFundProject_Data;
    
    [Parameter] 
    public string Company { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        await RefreshData();
        on = true;
    }
    
    private async Task RefreshData()
    {
        var sheetData = await GoogleSheetsService.Get(Company, "A", "20", "E", "24");
        
        if (sheetData != null)
        {
            title = (string) sheetData[0][0];
            GovFundProject_Data = sheetData;
        }
    }
}