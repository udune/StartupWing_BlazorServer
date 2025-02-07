using ApexCharts;
using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Dashboard;

public partial class SubscriptionManagement : ComponentBase
{
    private class SubscriptionManagement_Data
    {
        public string? Month { get; set; }
        public string? Total { get; set; }
        public string[] Subscriptions { get; set; } = new string[12];
    }

    private bool on;
    private string title;
    private readonly List<SubscriptionManagement_Data> SubscriptionManagement_Datas = new();
    private readonly List<string> Header_Datas = new();
    private ApexChartOptions<SubscriptionManagement_Data> option;
    
    [Parameter] 
    public string Company { get; set; }

    protected override async Task OnInitializedAsync()
    {
        option = new ApexChartOptions<SubscriptionManagement_Data>
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
            Markers = new Markers
            {
                FillOpacity = 0,
                StrokeOpacity = 0
            },
            Chart = new Chart
            {
                Toolbar = new Toolbar { Show = false }
            },
            PlotOptions = new PlotOptions
            {
                Bar = new PlotOptionsBar { ColumnWidth = "50%" }
            },
            Stroke = new Stroke
            {
                Width = new Size(0)
            },
            Legend = new Legend { Show = false },
            Yaxis =
            [
                new YAxis
                {
                    Labels = new YAxisLabels
                    {
                        Show = true,
                        Style = new AxisLabelStyle
                        {
                            FontWeight = 700
                        },
                        Formatter = "function (value) { " +
                                    "    if (value == null || value === undefined) return ''; " + // null 또는 undefined 처리
                                    "    var valueInTenThousand = value / 10000; " + // 만원 단위로 나누기
                                    "    var result = ''; " +

                                    // 만원 단위 처리
                                    "    var thousands = Math.floor(valueInTenThousand); " + // 만 단위 (천만원 기준이 아닌 만원 기준)
                                    "    var remainder = valueInTenThousand - thousands; " + // 나머지 처리
                                    "    var remaining = Math.round(remainder * 10); " + // 소수점 첫째 자리까지 처리

                                    // 만 단위 표시
                                    "    result += thousands; " +
                
                                    "    return result; " + // 결과 반환
                                    "}"
                    }
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
                Shared = true,
                Custom = "function({ series, seriesIndex, dataPointIndex, w }) { " +
                         "    var tooltipData = []; " +
                         "    series.forEach(function(data, idx) { " +
                         "        if (data[dataPointIndex] != null && !isNaN(data[dataPointIndex]) && data[dataPointIndex] !== undefined && data[dataPointIndex] !== 0) { " +
                         "            var value = data[dataPointIndex]; " +
                         "            if (value >= 10000) { " +  // 10000 이상인 값만 푸시
                         "                var valueInTenThousand = value / 10000; " +
                         "                var valueFormatted = Math.floor(valueInTenThousand); " +
                         "                tooltipData.push({ " +
                         "                    name: w.config.series[idx].name, " +
                         "                    value: valueFormatted, " +
                         "                    color: w.config.series[idx].color " +
                         "                }); " +
                         "            } " +
                         "        } " +
                         "    }); " +
                         "    tooltipData.sort(function(a, b) { " +
                         "        return b.value - a.value; " +  // 내림차순 정렬
                         "    }); " +
                         "    var tooltipHtml = '<div style=\"padding: 10px;\">'; " +
                         "    tooltipData.forEach(function(item) { " +
                         "        tooltipHtml += '<div style=\"font-size: 12px; margin-top: 5px; margin-bottom: 5px;\">' + " +
                         "                         '<span style=\"font-weight: bold;\">' + item.name + '</span>: ' + item.value + '만원</div>'; " +
                         "    }); " +
                         "    tooltipHtml += '</div>'; " +
                         "    return tooltipHtml; " +
                         "}"
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
        SubscriptionManagement_Datas.Clear();
        var sheetData = await GoogleSheetsService.Get(Company, "A", "49", "M", "65");

        if (sheetData != null)
        {
            title = (string)sheetData[0][0];
            Header_Datas.Add((string)sheetData[2][0]);

            for (var rowIdx = 1; rowIdx < sheetData.Count; rowIdx++)
            {
                for (var colIdx = 1; colIdx < sheetData[rowIdx].Count; colIdx++)
                {
                    switch (rowIdx)
                    {
                        case 1:
                            SubscriptionManagement_Datas.Add(new SubscriptionManagement_Data
                            {
                                Month = (string)sheetData[rowIdx][colIdx],
                                Total = ""
                            });
                            break;
                        case 2:
                            SubscriptionManagement_Datas[colIdx - 1].Total = (string)sheetData[rowIdx][colIdx];
                            break;
                        case 4:
                            Header_Datas.Add((string)sheetData[rowIdx][colIdx]);
                            break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                            SetSubscriptionData(SubscriptionManagement_Datas[rowIdx - 5], sheetData[rowIdx]);
                            break;
                    }
                }
            }
        }
    }

    void SetSubscriptionData(SubscriptionManagement_Data data, IList<object> rowData)
    {
        for (int i = 0; i < data.Subscriptions.Length; i++)
        {
            data.Subscriptions[i] = (string)rowData[i + 1];
        }
    }
}