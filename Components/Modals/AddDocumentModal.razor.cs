using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Modals;

public partial class AddDocumentModal : ComponentBase
{
    private void OnAddSubmit()
    {
        AddSubmitAction.InvokeAsync();
        Modal.Close();
    }
    
    private void OnChangeIsPublic(ChangeEventArgs e)
    {
        if (e.Value?.ToString() == "open")
        {
            AddDocumentData.IsPublic = true;
        }
        else if (e.Value?.ToString() == "close")
        {
            AddDocumentData.IsPublic = false;
        }
    }

    private void OnCancel()
    {
        Modal.Close();
    }
}