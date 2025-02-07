using ApexCharts;
using Microsoft.AspNetCore.Components;
using StartupWing_BlazorServer.Components.Manager;

namespace StartupWing_BlazorServer.Components.Dashboard;

public partial class CashFlow : ComponentBase
{
    public class CashFlow_Data
    {
        public string? Month { get; set; }
        public string? Income { get; set; }
        public string? Outcome { get; set; }
        public string? Balance { get; set; }
    }
    
    private bool on;
    private string title;
    
    public List<CashFlow_Data> CashFlow_Datas = new ();
    private List<string> Section_Datas = new ();
    private ApexChartOptions<CashFlow_Data> option = new ();
    private List<double> IncomeList = new();
    private List<double> BalanceList = new();

    [Parameter] 
    public string Company { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        option = new ApexChartOptions<CashFlow_Data>
        {
            Annotations = new Annotations
            {
                Texts =
                [
                    new AnnotationsText
                    {
                        X = 0,
                        Y = 10,
                        Text = "(단위: 만원)",
                        FontSize = 10,
                        FontWeight = 400
                    }
                ]
            },
            Stroke = new Stroke
            {
                LineCap = LineCap.Round,
                Width = new Size(3)
            },
            Chart = new Chart
            {
                Width = "100%",
                Toolbar  = new Toolbar
                {
                    Show = false
                }
            },
            PlotOptions = new PlotOptions
            {
                Bar = new PlotOptionsBar
                {
                    ColumnWidth = "50%"
                }
            },
            Yaxis =
            [
                new YAxis
                {
                    Labels = new YAxisLabels
                    {
                        Show = true,
                        Style = new AxisLabelStyle
                        {
                            Colors = ["#0099FF"],
                            FontWeight = 700
                        },
                        Formatter = "function (value) { " +
                                    "    if (value == null || value === undefined) return ''; " + // null 또는 undefined 처리
                                    "    var valueInTenThousand = value / 10000; " + // 만원 단위로 나누기
                                    "    var result = ''; " +

                                    // 만원 단위 처리
                                    "    var thousands = Math.floor(valueInTenThousand); " + // 만 단위 (천만원 기준이 아닌 만원 기준)
                                    "    var remainder = valueInTenThousand - thousands; " + // 나머지 처리
                                    "    var remaining = Math.floor(remainder * 10); " + // 소수점 첫째 자리까지 처리

                                    // 만 단위 표시
                                    "    result += thousands; " +
                
                                    "    return result - (result % 10); " + // 결과 반환
                                    "}"
                    },
                    Min=0
                },
                new YAxis
                {
                    Labels = new YAxisLabels
                    {
                        Show = false,
                        Formatter = "function (value) { " +
                                    "    if (value == null || value === undefined) return ''; " + // null 또는 undefined 처리
                                    "    var valueInTenThousand = value / 10000; " + // 만원 단위로 나누기
                                    "    var result = ''; " +

                                    // 만원 단위 처리
                                    "    var thousands = Math.floor(valueInTenThousand); " + // 만 단위 (천만원 기준이 아닌 만원 기준)
                                    "    var remainder = valueInTenThousand - thousands; " + // 나머지 처리
                                    "    var remaining = Math.floor(remainder * 10); " + // 소수점 첫째 자리까지 처리

                                    // 만 단위 표시
                                    "    result += thousands; " +
                
                                    "    return result - (result % 10); " + // 결과 반환
                                    "}"
                    },
                    Min=0
                },
                new YAxis
                {
                    Opposite = true,
                    Labels = new YAxisLabels
                    {
                        Show = true,
                        Style = new AxisLabelStyle
                        {
                            Colors = ["#FFD16D"],
                            FontWeight = 700
                        },
                        Formatter = "function (value) { " +
                                    "    if (value == null || value === undefined) return ''; " + // null 또는 undefined 처리
                                    "    var valueInTenThousand = value / 10000; " + // 만원 단위로 나누기
                                    "    var result = ''; " +

                                    // 만원 단위 처리
                                    "    var thousands = Math.floor(valueInTenThousand); " + // 만 단위 (천만원 기준이 아닌 만원 기준)
                                    "    var remainder = valueInTenThousand - thousands; " + // 나머지 처리
                                    "    var remaining = Math.floor(remainder * 10); " + // 소수점 첫째 자리까지 처리

                                    // 만 단위 표시
                                    "    result += thousands; " +
                
                                    "    return result - (result % 10); " + // 결과 반환
                                    "}"
                    },
                    Min=0
                }
            ],
            Xaxis = new XAxis
            {
                Labels = new XAxisLabels
                {
                    Formatter = "function (value) { if (value === undefined) {return '';} return value.toUpperCase();}"
                }
            },
            Tooltip = new Tooltip
            {
                Y = new TooltipY
                {
                    Formatter = "function (value) { " +
                                "    if (value == null || value === undefined) return ''; " + // null 또는 undefined 처리
                                "    var valueInTenThousand = value / 10000; " + // 만원 단위로 나누기
                                "    var result = ''; " +

                                // 만원 단위 처리
                                "    var thousands = Math.floor(valueInTenThousand); " + // 만 단위 (천만원 기준이 아닌 만원 기준)
                                "    var remainder = valueInTenThousand - thousands; " + // 나머지 처리
                                "    var remaining = Math.floor(remainder * 10); " + // 소수점 첫째 자리까지 처리

                                // 만 단위 표시
                                "    result += thousands >= 10000 ? Math.floor(thousands/10000) + '억' + (thousands%10000) + '만원' : thousands + '만원'; " +
                
                                "    return result; " + // 결과 반환
                                "}"
                }
            },
            DataLabels = new DataLabels
            {
                Formatter = "function(value, opts) { return  Number(value).toLocaleString(); }"
            }
        };
        
        await RefreshData();
        on = true;
    }
    
    private async Task RefreshData()
    {
        CashFlow_Datas.Clear();
        var sheetData = await GoogleSheetsService.Get(Company, "A", "14", "N", "18");
        
        if (sheetData != null)
        {
            title = (string) sheetData[0][0];

            for (int colIdx = 1; colIdx < sheetData[1].Count; colIdx++)
            {
                CashFlow_Datas.Add(new CashFlow_Data
                {
                    Month = (string) sheetData[1][colIdx],
                    Income = "",
                    Outcome = "",
                    Balance = ""
                });
            }
            
            for (int rowIdx = 2; rowIdx < sheetData.Count; rowIdx++)
            {
                Section_Datas.Add((string) sheetData[rowIdx][0]);

                for (int colIdx = 1; colIdx < sheetData[rowIdx].Count; colIdx++)
                {
                    var value = (string) sheetData[rowIdx][colIdx];
                    switch (rowIdx)
                    {
                        case 2:
                            CashFlow_Datas[colIdx - 1].Income = value;
                            IncomeList.Add(double.Parse(value));
                            break;
                        case 3:
                            CashFlow_Datas[colIdx - 1].Outcome = value;
                            IncomeList.Add(double.Parse(value));
                            break;
                        case 4:
                            CashFlow_Datas[colIdx - 1].Balance = value;
                            BalanceList.Add(double.Parse(value));
                            break;
                    }
                }
            }

            option.Yaxis[0].Max = IncomeList.Max() * 1.2f;
            option.Yaxis[1].Max = IncomeList.Max() * 1.2f;
            option.Yaxis[2].Max = BalanceList.Max() * 1.2f;
        }
    }
}