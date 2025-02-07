using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Modals;

public partial class Modal : ComponentBase
{
    private RenderFragment? Contents = null;

    private bool IsShow = false;
    private bool IsOutClose = false;
    
    public void Show()
    {
        IsShow = true;
        StateHasChanged();
    }

    public void Show<T>(Dictionary<string, object?>? parameters = null,bool isClose = false) where T : ComponentBase
    {
        Contents = CreateComponent<T>(parameters);
        IsShow = true;
        IsOutClose = isClose;
        StateHasChanged();
    }
    
    public void Show<T>(string Title, string Message, int ButtonCount = 2, Action? YesEvent = null, Action? NoEvent = null, bool isClose = false) where T : ComponentBase
    {
        Contents = CreateComponent<T>(new Dictionary<string, object?>
        {
            {"Title", Title},
            {"Message", Message},
            {"ButtonCount", ButtonCount},
            {"YesEvent", YesEvent},
            {"NoEvent", NoEvent}
        });
        IsShow = true;
        IsOutClose = isClose;
        StateHasChanged();
    }

    public void Close()
    {
        IsShow = false;
        Contents = null;
        StateHasChanged();
    }

    private RenderFragment CreateComponent<T>(Dictionary<string, object?>? parameters) where T : ComponentBase => builder =>
    {
        builder.OpenComponent<T>(0);
        builder.AddAttribute(1, "Modal", this);
        if (parameters != null)
        {
            foreach (var item in parameters)
            {
                builder.AddAttribute(2, item.Key, item.Value);
            }
        }

        builder.CloseComponent();
    };
    
    private void OnClick_Out()
    {
        if (IsShow && IsOutClose)
        {
            Close();
        }
    }

}
