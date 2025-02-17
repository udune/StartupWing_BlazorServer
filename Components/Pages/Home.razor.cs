using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Pages;

public partial class Home : ComponentBase
{
    private bool on;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetMyData(94);
            on = true;
            StateHasChanged();
            return;
            var cookie = await CookieService.GetAsync("accessToken");
            if (cookie != null)
            {
                var cookieToken = cookie.Value;
                if (!string.IsNullOrEmpty(cookieToken))
                {
                    DataManager.MyAccessToken = cookieToken;
                    var myId = TokenService.DecodeToken(cookieToken);
                    if (!string.IsNullOrEmpty(myId))
                    {
                        await SetMyData(int.Parse(myId));
                    }
                }
                else
                {
                    ClearUserData();
                }
            }
            else
            {
                ClearUserData();
            }
            
            on = true;
            StateHasChanged();
        }
    }
    
    private void ClearUserData()
    {
        DataManager.MyData = null;
        DataManager.MyAccessToken = string.Empty;
        Navigation.NavigateTo(Navigation.Uri, false);
    }
    
    private async Task SetMyData(int myIntId)
    {
        if (myIntId > 0)
        {
            DataManager.MyData = await APIService.GetUserData_ID(myIntId);
            Navigation.NavigateTo(Navigation.Uri, false);
        }
    }
}