using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace StartupWing_BlazorServer.Components.Dashboard;

public partial class TodayHoliday : ComponentBase
{
    public class TodayHoliday_Data
    {
        public string? Department { get; set; }
        public string? Count { get; set; }
        public List<RosterData> Roster { get; set; }
    }

    public class RosterData
    {
        public string Name;
        public string Date;
        public string Type;
    }

    private bool on;
    private string title;
    private string totalCount;
    private List<TodayHoliday_Data> TodayHolidayDatas = new();
    private List<RosterData> RosterDatas_Temp = new();
    private readonly List<string> Header_Datas = new();

    private List<RosterData> CurrentRoster = new ();
    
    [Parameter] 
    public string Company { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await RefreshData();
        on = true;
    }

    private async Task RefreshData()
    {
        var sheetData = await GoogleSheetsService.Get(Company, "A", "5", "N", "12");

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
                            TodayHolidayDatas.Add(new TodayHoliday_Data { Department = (string)sheetData[rowIdx][colIdx], Count = "", Roster = new List<RosterData>() });
                            break;
                        case 2:
                            TodayHolidayDatas[colIdx - 1].Count = (string)sheetData[rowIdx][colIdx];
                            break;
                        case 4:
                            RosterDatas_Temp.Add(new RosterData { Name = (string)sheetData[rowIdx][colIdx], Date = "", Type = "" });
                            break;
                        case 6:
                            RosterDatas_Temp[colIdx - 1].Date = (string)sheetData[rowIdx][colIdx];
                            break;
                        case 7:
                            RosterDatas_Temp[colIdx - 1].Type = (string)sheetData[rowIdx][colIdx];
                            break;
                    }
                }

                if (rowIdx >= 4)
                {
                    Header_Datas.Add((string)sheetData[rowIdx][0]);
                }
            }

            if (RosterDatas_Temp.Count > 0)
            {
                for (var colIdx = 1; colIdx < sheetData[5].Count; colIdx++)
                {
                    var departmentData = TodayHolidayDatas.Find(x => x.Department == (string)sheetData[5][colIdx]);
                    if (departmentData != null)
                    {
                        departmentData.Roster.Add(new RosterData
                        {
                            Name = RosterDatas_Temp[colIdx - 1].Name, 
                            Date = RosterDatas_Temp[colIdx - 1].Date, 
                            Type = RosterDatas_Temp[colIdx - 1].Type
                        });
                    }
                }
            }
        }
    }

    private void Show(string? count, List<RosterData> roster)
    {
        if (!string.IsNullOrEmpty(count) && int.Parse(count) > 0)
        {
            CurrentRoster = roster;
            Js.InvokeVoidAsync("TodayHoliday_Show");
        }
    }

    private void Hide()
    {
        Js.InvokeVoidAsync("TodayHoliday_Hide");
    }
}