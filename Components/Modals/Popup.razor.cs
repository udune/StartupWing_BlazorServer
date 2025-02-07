using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Modals;

public partial class Popup : ComponentBase
{
    
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        YesEvent ??= () =>
        {
            Console.WriteLine("확인");
            Modal?.Close();
        };

        NoEvent ??= () =>
        {
            Console.WriteLine("취소");
            Modal?.Close();
        };
    }
    
}