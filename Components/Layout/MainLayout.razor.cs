using StartupWing_BlazorServer.Components.Modals;

namespace StartupWing_BlazorServer.Components.Layout;

public partial class MainLayout
{
    SideMenu sideMenu;
    private Modal login;
    private Modal popup;
    
    public void OnLogin()
    {
        login.Show<LoginModal>(null, true);
    }

    public void OnLogout()
    {
        popup.Show<Popup>("", "로그아웃하시겠어요?", 2, () =>
        {
            DataManager.MyData = null;
            DataManager.MyAccessToken = string.Empty;
            Navigation.NavigateTo(Navigation.Uri, false);
        });
    }
}