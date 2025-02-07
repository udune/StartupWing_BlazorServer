using Microsoft.AspNetCore.Components;
using StartupWing_BlazorServer.Components.Dashboard;

namespace StartupWing_BlazorServer.Components.Pages;

public partial class Dashboard : ComponentBase
{
    [Parameter] 
    public string PageType { get; set; } = "home";

    [Parameter] 
    public string Company { get; set; } = "데모";
    
    bool modify;
    
    private bool[] checkArray_init = new bool[9];
    private bool[] checkArray_temp = new bool[9];
    private bool[] checkArray = new bool[9];
    
    private List<string> itemList_init = new ();
    private List<string> itemList_temp = new ();
    private List<string> itemList = new ();

    const string MC = "a_직원 구성원";
    const string TH = "b_금일 휴가자";
    const string CF = "c_현금 흐름";
    const string CM = "d_계약 관리";
    const string FO = "e_재무 개요";
    const string SM = "f_구독 서비스";
    const string LE = "g_법정의무교육 이수";
    const string GP = "h_정부지원과제";
    const string MP = "i_월별 실적 관리";

    private List<string> itemNameList = [MC, TH, CF, CM, FO, SM, LE, GP, MP];
    private Dictionary<string, Type> componentDict = new()
    {
        {MC, typeof(MemberCount)},
        {TH, typeof(TodayHoliday)},
        {CF, typeof(CashFlow)},
        {CM, typeof(ContractManagement)},
        {FO, typeof(FinancialOverview)},
        {SM, typeof(SubscriptionManagement)},
        {LE, typeof(LegalEducation)},
        {GP, typeof(GovFundProject)},
        {MP, typeof(MonthlyPerformanceManagement)}
    };

    protected override void OnInitialized()
    {
        base.OnInitialized();
        
        // 초기화면 설정
        itemList = [MC, TH, CF, CM, FO, SM, LE, GP, MP];
        checkArray = [
            true, // (1)
            true, // (2) 
            true, // (3) 
            true, // (4) 
            true, // (5) 
            true, // (6) 
            true, // (7) 
            true, // (8) 
            true, // (9) 
        ];
    }

    // RenderFragment : razor 컴포넌트를 @code 단에서 불러오거나 호출해서 사용할 수 있다.
    private RenderFragment GetItem(string item, string? value = null) => builder =>
    {
        if (componentDict.TryGetValue(item, out var componentType))
        {
            builder.OpenComponent(0, componentType);
            if (value != null)
            {
                builder.AddAttribute(1, "Company", value);
            }   
            builder.CloseComponent();
        }
    };

    private void OnCheckBox(int index)
    {
        var itemLabel = itemNameList[index];
        if (!checkArray_temp[index])
        {
            if (!itemList_temp.Contains(itemLabel))
            {
                itemList_temp.Add(itemLabel);
            }
        }
        else
        {
            itemList_temp.Remove(itemLabel);
        }
    }
    
    private void OpenModify()
    {
        itemList_init = new List<string>(itemList);
        itemList_temp = new List<string>(itemList);
        
        Array.Copy(checkArray, checkArray_init, checkArray.Length);
        Array.Copy(checkArray, checkArray_temp, checkArray.Length);
        
        modify = true;
    }
    
    private void CloseModify()
    {
        itemList_temp = new List<string>(itemList_init);
    
        Array.Copy(checkArray_init, checkArray_temp, checkArray.Length);
        modify = false;
    }
    
    private void Save()
    {
        itemList = new List<string>(itemList_temp);
        itemList.Sort();
        
        Array.Copy(checkArray_temp, checkArray, checkArray.Length);
        modify = false;
    }
}