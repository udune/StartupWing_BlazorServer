using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Layout;

public partial class SideMenu : ComponentBase
{
    List<string> text_class_list = [ "text-item-select", "text-item", "text-item", "text-item", "text-item" ];
    List<string> image_class_list = [ "image-item-select", "image-item", "image-item", "image-item", "image-item" ];

    private void On(int idx)
    {
        for (int i = 0; i < text_class_list.Count; i++)
        {
            if (i == idx)
            {
                text_class_list[i] = "text-item-select";
                image_class_list[i] = "image-item-select";
                continue;
            }
            
            text_class_list[i] = "text-item";
            image_class_list[i] = "image-item";
        }
        StateHasChanged();
    }

    public void Off()
    {
        for (int i = 0; i < text_class_list.Count; i++)
        {
            if (i == 0)
            {
                text_class_list[i] = "text-item-select";
                image_class_list[i] = "image-item-select";
                continue;
            }
            
            text_class_list[i] = "text-item";
            image_class_list[i] = "image-item";
        }
        StateHasChanged();
    }
}