using ApexCharts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace StartupWing_BlazorServer.Components.Dashboard;

public partial class LegalEducation : ComponentBase
{
    private class LegalEducation_Data
    {
        public string? Status { get; set; }
        public string? Percent { get; set; }
    }
    
    private bool on;
    private string title;
    private List<LegalEducation_Data> LegalEducation_FirstDatas = new ();
    private List<LegalEducation_Data> LegalEducation_SecondDatas = new ();
    private List<string> NotCompleted_FirstDatas = new ();
    private List<string> NotCompleted_SecondDatas = new ();
    private List<string> Section_Datas = new ();
    
    [Parameter] 
    public string Company { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        await RefreshData();
        on = true;
    }

    private string GetPointColor(LegalEducation_Data data)
    {
        if (data.Status == null)
        {
            return "#FF6D6D";
        }

        switch (data.Status)
        {
            case var status 
                when status == LegalEducation_FirstDatas.ElementAtOrDefault(0)?.Status:
                return "#0099FF";
            case var status 
                when status == LegalEducation_SecondDatas.ElementAtOrDefault(1)?.Status:
                return "#FF6D6D";
            default:
                return "#FF6D6D";
        }
    }

    private ApexChartOptions<LegalEducation_Data> GetOption()
    {
        return new ApexChartOptions<LegalEducation_Data>
        {
            Chart = new Chart
            {
                Toolbar  = new Toolbar
                {
                    Show = false
                },
                Height = 200,
                Width = "75%"
            },
            Title = new Title
            {
                Align = Align.Center,
                Style = new TitleStyle
                {
                    FontSize = "12px",
                    FontWeight = "400",
                    Color = "#2C2C2C"
                }
            },
            PlotOptions = new PlotOptions
            {
                Pie = new PlotOptionsPie
                {
                    Donut = new PlotOptionsDonut
                    {
                        Size = "50%"
                    }
                }
            },
            Legend = new Legend
            {
                Show = true,
                Position = LegendPosition.Bottom,
                HorizontalAlign = Align.Center,
                Labels = new LegendLabels
                {
                    Colors = ["#000000"]
                }
            },
            DataLabels = new DataLabels
            {
                DropShadow = new DropShadow
                {
                    Enabled = false
                },
                Background = new DataLabelsBackground
                {
                    Enabled = true,
                    Padding = 10,
                    BorderWidth = 5,
                    BorderColor = "f0f0f0",
                    BorderRadius = 5,
                    ForeColor = "#000000"
                },
                Formatter = "function(val, opts) { return Math.floor(val) + '%'; }"
            },
            Tooltip = new Tooltip
            {
                Enabled = false
            },
            Stroke = new Stroke
            {
                Show = false
            }
        };
    }
    
    private List<LegalEducation_Data> GetData(int index)
    {
        return index == 0 ? LegalEducation_FirstDatas : LegalEducation_SecondDatas;
    }

    private List<string> GetNotCompletedData(int index)
    {
        return index == 0 ? NotCompleted_FirstDatas : NotCompleted_SecondDatas;
    }

    private async Task RefreshData()
    {
        LegalEducation_FirstDatas.Clear();
        LegalEducation_SecondDatas.Clear();
        Section_Datas.Clear();
        
        var sheetData = await GoogleSheetsService.Get(Company, "A", "67", "C", "76");
        var sheetData_List = await GoogleSheetsService.Get(Company, "E", "68", "F", "95");
        
        if (sheetData != null)
        {
            title = (string) sheetData[0][0];
            Section_Datas.Add((string) sheetData[1][0]);
            Section_Datas.Add((string) sheetData[6][0]);

            ProcessData(sheetData, 2, 3, LegalEducation_FirstDatas);
            ProcessData(sheetData, 7, 8, LegalEducation_SecondDatas);
        }

        if (sheetData_List != null)
        {
            for (int rowIdx = 1; rowIdx < sheetData_List.Count; rowIdx++)
            {
                if (sheetData_List[rowIdx].Count > 0 && !string.IsNullOrEmpty((string)sheetData_List[rowIdx][0]))
                {
                    NotCompleted_FirstDatas.Add((string)sheetData_List[rowIdx][0]);
                }
                
                if (sheetData_List[rowIdx].Count > 1 && !string.IsNullOrEmpty((string)sheetData_List[rowIdx][1]))
                {
                    NotCompleted_SecondDatas.Add((string)sheetData_List[rowIdx][1]);
                }
            }
        }
    }

    private void ProcessData(IList<IList<object>> sheetData, int first_row, int second_row, List<LegalEducation_Data> dataList)
    {
        dataList.Add(new LegalEducation_Data { Status = (string) sheetData[first_row][0], Percent = ParsePercentage(sheetData[first_row][2]).ToString() });
        dataList.Add(new LegalEducation_Data { Status = (string) sheetData[second_row][0], Percent = ParsePercentage(sheetData[second_row][2]).ToString() });
    }

    private int ParsePercentage(object data)
    {
        var percent = (string) data;
        var number = Convert.ToDouble(percent.Replace("%", ""));
        return (int) Math.Round(number);
    }

    private void ShowTooltip(int index)
    {
        Js.InvokeVoidAsync($"LegalEducation_Show_{index}");
    }

    private void HideTooltip(int index)
    {
        Js.InvokeVoidAsync($"LegalEducation_Hide_{index}");
    }
}