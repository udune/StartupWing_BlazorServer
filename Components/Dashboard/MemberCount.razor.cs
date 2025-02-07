using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Dashboard;

public partial class MemberCount : ComponentBase
{
    public class MemberCount_Data
    {
        public string? Department { get; set; }
        public string? Count { get; set; }
    }
    
    private bool on;
    private string title;
    private string totalCount;
    private List<MemberCount_Data> MemberCount_Datas = new ();
    
    [Parameter] 
    public string Company { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        await RefreshData();
        on = true;
    }
    
    private async Task RefreshData()
    {
        var sheetData = await GoogleSheetsService.Get(Company, "A", "1", "N", "3");
        
        if (sheetData != null)
        {
            title = (string) sheetData[0][0];
            totalCount = (string) sheetData[0][1];

            for (var rowIdx = 1; rowIdx < sheetData.Count; rowIdx++)
            {
                for (var colIdx = 1; colIdx < sheetData[rowIdx].Count; colIdx++)
                {
                    switch (rowIdx)
                    {
                        case 1:
                            MemberCount_Datas.Add(new MemberCount_Data { Department = (string)sheetData[rowIdx][colIdx], Count = "" });
                            break;
                        case 2:
                            MemberCount_Datas[colIdx - 1].Count = (string)sheetData[rowIdx][colIdx];
                            break;
                    }
                }
            }
        }
    }
}