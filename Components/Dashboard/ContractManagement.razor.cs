using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Dashboard;

public partial class ContractManagement : ComponentBase
{
    private bool on;
    private string title;
    private string unit;
    private IList<IList<object>> ContractManagement_Data;
    
    [Parameter] 
    public string Company { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        await RefreshData();
        on = true;
    }
    
    private async Task RefreshData()
    {
        var sheetData = await GoogleSheetsService.Get(Company, "A", "32", "E", "36");
        
        if (sheetData != null)
        {
            title = (string) sheetData[0][0];
            unit = (string)sheetData[0][4];
            ContractManagement_Data = sheetData;
        }
    }
}