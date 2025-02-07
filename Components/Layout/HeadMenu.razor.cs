using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Layout;

public partial class HeadMenu : ComponentBase
{
    [Parameter]
    public SideMenu SideMenu { get; set; }
    [Parameter]
    public MainLayout MainLayout { get; set; }

    private void Home()
    {
        SideMenu.Off();
    }
}