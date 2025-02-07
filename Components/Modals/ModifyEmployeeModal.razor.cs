using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Modals;

public partial class ModifyEmployeeModal : ComponentBase
{
    private void OnModifySubmit()
    {
        ModifySubmitAction.InvokeAsync();
        Modal.Close();
    }

    private void OnChangeRole(ChangeEventArgs e)
    {
        ModifyEmployeeData.Role = e.Value?.ToString();
    }

    private void OnCancel()
    {
        Modal.Close();
    }

    private void OnRemoveSubmit()
    {
        OnRemoveSubmitAction.InvokeAsync();
    }
}